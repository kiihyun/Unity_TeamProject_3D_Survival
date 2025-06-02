using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] private Transform enemyParent;
    [SerializeField] private float minSpawnPos;
    [SerializeField] private float maxSpawnPos;
    [SerializeField] private float respawnDelay =  10f;
    [SerializeField] private List<EnemyDataEntry> enemyEntries;

    void Start()
    {
        foreach (var entry in enemyEntries)
        {
            SpawnEnemies(entry, entry.data.spawnCount);
        }
    }

    void SpawnEnemies(EnemyDataEntry entry, int count)
    {
        var data = entry.data;
        for (int i = 0; i < count; i++)
        {
            Vector2 randCircle = Random.insideUnitCircle.normalized * Random.Range(minSpawnPos, maxSpawnPos);
            Vector3 rawPos = entry.spawnPoint.position + new Vector3(randCircle.x, 0f, randCircle.y);

            Vector3 spawnPos = GetValidNavMeshPosition(rawPos, 2f); // NavMesh 위 위치로 보정

            int randomIndex = Random.Range(0, data.prefab.Length);
            GameObject obj = ObjectPoolManager.Instance.GetObjectByPrefab(data.prefab[randomIndex], null, spawnPos);
            obj.transform.SetParent(enemyParent);
            NavMeshAgent agent = obj.GetComponent<NavMeshAgent>();
            if (agent != null)
            {
                agent.enabled = false;
                agent.transform.position = spawnPos;
                agent.enabled = true;
            }

            if (NavMesh.SamplePosition(spawnPos, out NavMeshHit hit, 100f, NavMesh.AllAreas))
            {
                spawnPos = hit.position;
            }
            else
            {
                Debug.LogWarning($"No NavMesh nearby! spawnPos: {spawnPos}");
            }
            obj.SetActive(true);

            entry.activeEnemies.Add(obj);

            //리스폰
            Enemy enemy = obj.GetComponent<Enemy>();
            //enemy 초기화

            if (enemy != null)
            {
                enemy.data = data;
                enemy.OnDieCallback = (deadObj) =>
                {
                    entry.activeEnemies.Remove(deadObj);
                    StartCoroutine(RespawnOneAfterDelay(entry));
                };
            }
            else // BaseEnemy로 상속받아 중복코드 정리 예정
            {
                PassiveAnimal animal = obj.GetComponent<PassiveAnimal>();
                if (animal != null)
                {
                    animal.data = data;
                    animal.OnDieCallback = (deadObj) =>
                    {
                        entry.activeEnemies.Remove(deadObj);
                        StartCoroutine(RespawnOneAfterDelay(entry));
                    };
                }
            }
        }
    }

    IEnumerator RespawnOneAfterDelay(EnemyDataEntry entry)
    {
        yield return new WaitForSeconds(respawnDelay);
        // 현재 활성 적 수가 스폰 제한보다 적을 경우에만 리스폰
        if (entry.activeEnemies.Count < entry.data.spawnCount)
        {
            SpawnEnemies(entry, 1);
        }
    }
    Vector3 GetValidNavMeshPosition(Vector3 position, float maxDistance = 2f)
    {
        if (NavMesh.SamplePosition(position, out NavMeshHit hit, maxDistance, NavMesh.AllAreas))
        {
            return hit.position;
        }
        else
        {
            Debug.LogWarning($"NavMesh 위에 유효한 위치가 없음: {position}");
            return position; // 실패 시 원래 위치 사용 (오류는 날 수 있음)
        }
    }
}
