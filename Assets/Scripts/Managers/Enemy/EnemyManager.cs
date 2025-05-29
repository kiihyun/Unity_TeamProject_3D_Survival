using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] private Transform enemySpawnPos;
    [SerializeField] private float minSpawnPos;
    [SerializeField] private float maxSpawnPos;
    [SerializeField] private float respawnDelay = 5f;
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
            Vector3 spawnPos = new(randCircle.x, 0f, randCircle.y);

            GameObject obj = ObjectPoolManager.Instance.GetObjectByPrefab(data.prefab, enemySpawnPos, spawnPos);
            obj.SetActive(true);

            entry.activeEnemies.Add(obj);

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
        }
    }

    IEnumerator RespawnOneAfterDelay(EnemyDataEntry entry)
    {
        yield return new WaitForSeconds(respawnDelay);
        SpawnEnemies(entry, 1);
    }
}
