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

    private List<GameObject> activeEnemies = new List<GameObject>();

    void Start()
    {
        for (int i = 0; i < enemyCount; i++)
        {
            SpawnEnemyRandomPosition();
        }
        StartCoroutine(RespawnEnemy());
    }
    IEnumerator RespawnEnemy()
    {
        while (true)
        {
            yield return new WaitForSeconds(enemyRespawnTime);
            // 살아있는 적 수가 부족할 때만 스폰
            if (activeEnemies.Count < enemyCount)
            {
                SpawnEnemyRandomPosition();
            }
        }
    }

    void SpawnEnemyRandomPosition()
    {   
        Vector2 randCircle = Random.insideUnitCircle.normalized * Random.Range(minSpawnPos, maxSpawnPos);
        Vector3 spawnPos = new Vector3(randCircle.x, 0f, randCircle.y);
        GameObject obj = ObjectPoolManager.Instance.GetObjectByPrefab(enemyPrefab, enemySpawnPos, spawnPos);
        if (obj != null)
        {
            activeEnemies.Add(obj);

            // Enemy에 자기 파괴 시 EnemyManager에 알리는 코드 추가
            Enemy enemy = obj.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.OnDieCallback = OnEnemyDied;
            }
        }
    }

    private void OnEnemyDied(GameObject enemyObj)
    {
        activeEnemies.Remove(enemyObj);
    }
}
