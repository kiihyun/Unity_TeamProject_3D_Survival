using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] private Transform enemyParent;
    [SerializeField] private float minSpawnPos;
    [SerializeField] private float maxSpawnPos;
    [SerializeField] private float respawnDelay =  5f;
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
            Vector3 spawnPos = entry.spawnPoint.localPosition+ new Vector3(randCircle.x, 0f, randCircle.y);

            GameObject obj = ObjectPoolManager.Instance.GetObjectByPrefab(data.prefab, null, spawnPos);
            obj.transform.SetParent(enemyParent);
            obj.SetActive(true);

            entry.activeEnemies.Add(obj);

            //리스폰
            Enemy enemy = obj.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.data = data;
                enemy.OnDieCallback = (deadObj) =>
                {
                    entry.activeEnemies.Remove(deadObj);
                    deadObj.SetActive(false);
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
                        deadObj.SetActive(false);
                        StartCoroutine(RespawnOneAfterDelay(entry));
                    };
                }
            }
        }
    }

    IEnumerator RespawnOneAfterDelay(EnemyDataEntry entry)
    {
        yield return new WaitForSeconds(respawnDelay);
        SpawnEnemies(entry, 1);
    }
}
