using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public enum AIState // 임시, 추후 이넘스크립트로 이동
{
    Idle,
    Wandering,
    Attacking,
    Running,
    Fleeing,
}

public class Enemy : MonoBehaviour, IDamagable
{
    [Header("Stats")]
    private int curHealth;
    public ItemData[] dropOnDeath;

    [Header("AI")]
    private NavMeshAgent agent;
    private AIState aiState;

    [Header("Combat")]
    private float lastAttackTime;
    private float playerDistance;
    private bool isDead;


    [Header("Data")]
    public EnemyDataSO data;

    private Animator animator;
    private SkinnedMeshRenderer[] meshRenderers;

    public System.Action<GameObject> OnDieCallback;
    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        meshRenderers = GetComponentsInChildren<SkinnedMeshRenderer>();

        NavMeshUtility.TrySnapToNavMesh(transform); // NavMesh 보정
    }

    void Start()
    {
        SetState(AIState.Wandering);

        curHealth = data.maxHealth;
    }

    void Update()
    {
        playerDistance = Vector3.Distance(transform.position, PlayerManager.Instance.player.transform.position);

        animator.SetBool("Moving", aiState != AIState.Idle);

        switch (aiState)
        {
            case AIState.Idle:
            case AIState.Wandering:
                PassiveUpdate();
                break;
            case AIState.Attacking:
                AttackingUpdate();
                break;
        }
    }
    public void SetState(AIState state)
    {
        if (aiState == state) return; //  동일한 상태면 무시
        aiState = state;

        switch (aiState)
        {
            case AIState.Idle:
                agent.speed = data.walkSpeed;
                agent.isStopped = true;
                break;
            case AIState.Wandering:
                agent.speed = data.walkSpeed;
                agent.isStopped = false;
                break;
            case AIState.Attacking:
                agent.speed = data.runSpeed;
                agent.isStopped = false;
                break;
        }
        animator.speed = agent.speed / data.walkSpeed;
    }

    void PassiveUpdate()
    {
        if (!agent.isOnNavMesh)
            Debug.LogWarning($"{gameObject.name} is not on a NavMesh!");
        if (agent.isOnNavMesh && aiState == AIState.Wandering && agent.remainingDistance < 0.1f)
        {
            SetState(AIState.Idle);
            Invoke("WanderToNewLocation", Random.Range(data.minWanderWaitTime, data.maxWanderWaitTime));
        }

        if (playerDistance < data.detectDistance && aiState != AIState.Attacking)
        {
            SetState(AIState.Attacking);
        }
    }

    void WanderToNewLocation()
    {
        if (aiState != AIState.Idle) return;

        SetState(AIState.Wandering);
        agent.SetDestination(GetWanderLocation());
    }

    Vector3 GetWanderLocation()
    {
        NavMeshHit hit;

        do
        {
            NavMesh.SamplePosition(transform.position + (Random.onUnitSphere * Random.Range(data.minWanderDistance, data.maxWanderDistance)), out hit, data.maxWanderDistance, NavMesh.AllAreas);
        }
        while (Vector3.Distance(transform.position, hit.position) < data.detectDistance);
        return hit.position;
    }

    void AttackingUpdate()
    {
        if (isDead) return;

        if (playerDistance < data.attackDistance && IsPlayerInFieldOfView())
        {
            agent.isStopped = true;
            Debug.Log($"agent.isStopped : {agent.isStopped}");
            if (Time.time - lastAttackTime > data.attackRate)
            {
                lastAttackTime = Time.time;
                PlayerManager.Instance.player.GetComponent<IDamagable>().TakePhysicalDamage(data.damage);
                Debug.Log("좀비가 공격");
                animator.speed = 1;
                animator.SetTrigger("Attack");
            }
            return;
        }
        else
        { // 공격 범위 밖 감지범위 안
            if (playerDistance < data.detectDistance)
            {
                agent.isStopped = false;
                NavMeshPath path = new NavMeshPath();
                if (agent.CalculatePath(PlayerManager.Instance.player.transform.position, path))
                {
                    agent.SetDestination(PlayerManager.Instance.player.transform.position);
                }
                else
                {
                    agent.SetDestination(transform.position);
                    agent.isStopped = true;
                    SetState(AIState.Wandering);
                }
            }
            else //감지 범위 밖
            {
                agent.SetDestination(transform.position);
                agent.isStopped = true;
                SetState(AIState.Wandering);
            }
        }
    }

    bool IsPlayerInFieldOfView()
    {
        Vector3 directionToPlayer = PlayerManager.Instance.player.transform.position - transform.position;
        float angle = Vector3.Angle(transform.forward, directionToPlayer);
        return angle < data.fieldOfView * 0.5f;
    }

    public void TakePhysicalDamage(int damage)
    {
        StartCoroutine(DamageFlash());
        curHealth -= damage;
        Debug.Log($"enemy {damage}피해받음 {curHealth}체력남음");
        if (curHealth <= 0)
        {
            for (int i = 0; i < meshRenderers.Length; i++)
            {
                meshRenderers[i].material.color = Color.white;
            }

            Die();

        }
    }
    void Die()
    {
        for (int i = 0; i < dropOnDeath.Length; i++)
        {
            Instantiate(dropOnDeath[i].dropPrefab, transform.position + Vector3.up * 2, Quaternion.identity);
        }
        // Pool에 반환하거나 파괴 전
        OnDieCallback?.Invoke(this.gameObject);
        StartCoroutine(DieCoroutine());
        Debug.Log("enemy Die");
        agent.enabled = false;
        isDead = true;
    }

    IEnumerator DamageFlash()
    {
        for (int i = 0; i < meshRenderers.Length; i++)
        {
            meshRenderers[i].material.color = new Color(1.0f, 0.6f, 0.6f);
        }
        yield return new WaitForSeconds(0.1f);

        for (int i = 0; i < meshRenderers.Length; i++)
        {
            meshRenderers[i].material.color = Color.white;
        }
    }

    IEnumerator DieCoroutine()
    {
        Debug.Log("die코루틴");
        animator.SetTrigger("Die"); // 죽는 애니메이션

        yield return new WaitForSeconds(5f); // 죽는 애니메이션 길이만큼 대기

        OnDieCallback?.Invoke(this.gameObject); //  리스폰 트리거

        gameObject.SetActive(false); // 여기서 비활성화
    }

    public static class NavMeshUtility
    {
        public static bool TrySnapToNavMesh(Transform objTransform, float maxDistance = 2f)
        {
            NavMeshHit hit;
            if (NavMesh.SamplePosition(objTransform.position, out hit, maxDistance, NavMesh.AllAreas))
            {
                objTransform.position = hit.position;
                return true;
            }
            return false;
        }
    }

    public void Init() // 초기화
    {
        curHealth = data.maxHealth;
        isDead = false;
        gameObject.SetActive(true);

        if (agent != null)
        {
            agent.enabled = true;
            agent.isStopped = false;
        }

        if (animator != null)
        {
            animator.Rebind(); // 애니메이션 초기화
            animator.Update(0f); // 즉시 반영
        }

        // 기타 상태값 초기화
    }
}
