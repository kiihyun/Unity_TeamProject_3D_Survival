using UnityEngine;
using UnityEngine.AI;

[CreateAssetMenu(fileName = "EnemyData", menuName = "Enemy/EnemyData", order = 0)]
public class EnemyDataSO : ScriptableObject
{


    [Header("Stats")]
    public string enemyName;
    public int maxHealth;
    public float walkSpeed;
    public float runSpeed;
    public int spawnCount;

    public GameObject[] prefab;
    public ItemData[] dropOnDeath;

    [Header("AI")]
    public float detectDistance;

    [Header("Wandering")]
    public float minWanderDistance;
    public float maxWanderDistance;
    public float minWanderWaitTime;
    public float maxWanderWaitTime;

    [Header("Combat")]
    public int damage;
    public float attackRate;
    public float attackDistance;
    public float fieldOfView = 120f;
}
