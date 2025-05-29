using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PassiveAnimal : MonoBehaviour, IDamagable
{
    [Header("Stats")]
    private int curHealth;
    public ItemData[] dropOnDeath;
    private float lastFleeTime; // 마지막 도망 시간
    public float fleeCooldown = 2f; // 도망 재시도까지의 지연

    [Header("AI")]
    private NavMeshAgent agent;
    private AIState aiState;

    [Header("Combat")]
    private float playerDistance;


    [Header("Data")]
    public EnemyDataSO data;

    private Animator animator;
    private SkinnedMeshRenderer[] meshRenderers;

    public System.Action<GameObject> OnDieCallback;
    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        meshRenderers = GetComponentsInChildren<SkinnedMeshRenderer>();
    }

    void Start()
    {
        SetState(AIState.Wandering);
        curHealth = data.maxHealth;
    }

    void Update()
    {
        playerDistance = Vector3.Distance(transform.position, PlayerManager.Instance.player.transform.position);

        switch (aiState)
        {
            case AIState.Idle:
            case AIState.Wandering:
                PassiveUpdate();
                break;
            case AIState.Running:
                RunningUpdate();
                break;
        }
    }

    private void RunningUpdate()
    {
        if (playerDistance < data.detectDistance)
        {
            if (Time.time > lastFleeTime + fleeCooldown && agent.remainingDistance < 1f)
            {
                agent.SetDestination(GetFleeLocation());
                lastFleeTime = Time.time; // 다음 도망은 쿨타임 이후에 가능
            }

        }
        else
        {
            SetState(AIState.Wandering);
        }
    }

    public void SetState(AIState state)
    {
        if (aiState == state) return; //중복전환 방지
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
            case AIState.Running:
                agent.speed = data.runSpeed;
                agent.isStopped = false;
                break;
        }
    }

    void PassiveUpdate()
    {
        if (aiState == AIState.Wandering && agent.remainingDistance < 0.1f)
        {
            SetState(AIState.Idle);
            Invoke(nameof(WanderToNewLocation), Random.Range(data.minWanderWaitTime, data.maxWanderWaitTime));
        }

        if (playerDistance < data.detectDistance && Time.time > lastFleeTime + fleeCooldown)
        {
            SetState(AIState.Running);
            lastFleeTime = Time.time; // 도망 간 시점 저장
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
    Vector3 GetFleeLocation()
    {
        Vector3 dirFromPlayer = (transform.position - PlayerManager.Instance.player.transform.position).normalized;
        Vector3 fleeTarget = transform.position + dirFromPlayer * data.maxWanderDistance;

        if (NavMesh.SamplePosition(fleeTarget, out NavMeshHit hit, data.maxWanderDistance, NavMesh.AllAreas))
            return hit.position;
        return transform.position;
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
        CancelInvoke();
        for (int i = 0; i < dropOnDeath.Length; i++)
        {
            Instantiate(dropOnDeath[i].dropPrefab, transform.position + Vector3.up * 2, Quaternion.identity);
        }
        // Pool에 반환하거나 파괴 전
        OnDieCallback?.Invoke(this.gameObject);
        gameObject.SetActive(false); // 혹은 ObjectPool 반환
        Debug.Log("enemy Die");
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
}
