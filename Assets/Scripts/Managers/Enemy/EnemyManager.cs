using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private Transform enemySpawnPos;
    [SerializeField] private float minSpawnPos;
    [SerializeField] private float maxSpawnPos;
    [SerializeField] private int enemyCount;
    [SerializeField] private int enemyRespawnTime;

    void Start()
    {
        for (int i = 0; i < enemyCount; i++)
        {
            Vector2 randCircle = Random.insideUnitCircle.normalized * Random.Range(minSpawnPos, maxSpawnPos);
            Vector3 spawnPos = new Vector3(randCircle.x, 0f, randCircle.y);
            GameObject obj = ObjectPoolManager.Instance.GetObjectByPrefab(enemyPrefab, enemySpawnPos, spawnPos);
        }
        StartCoroutine(RespawnEnemy());
    }
    IEnumerator RespawnEnemy()
    {
        while (true)
        {
            yield return new WaitForSeconds(enemyRespawnTime);
            Vector2 randCircle = Random.insideUnitCircle.normalized * Random.Range(minSpawnPos, maxSpawnPos);
            Vector3 spawnPos = new Vector3(randCircle.x, 0f, randCircle.y);
            GameObject obj = ObjectPoolManager.Instance.GetObjectByPrefab(enemyPrefab, enemySpawnPos, spawnPos);
        }
    }
}
