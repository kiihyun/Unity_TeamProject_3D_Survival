using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private Transform enemySpwanPos;
    [SerializeField] private float minSpwanPos;
    [SerializeField] private float maxSpwanPos;
    [SerializeField] private int enemyCount;
    
    private List<Enemy> enemiyList = new List<Enemy>();
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < enemyCount; i++)
        {
            Vector2 randCircle = Random.insideUnitCircle.normalized * Random.Range(minSpwanPos, maxSpwanPos);
            Vector3 spawnPos = enemySpwanPos.position + new Vector3(randCircle.x, 0f, randCircle.y);

            GameObject obj = Instantiate(enemyPrefab, spawnPos, Quaternion.identity);
            enemiyList.Add(obj.GetComponent<Enemy>());
        }
        

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
