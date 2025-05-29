using System.Collections.Generic;
using UnityEngine;
public enum EnemyType
{
    Normal,
    Boss,
    Rabbit,
    // 추가 가능
}

[CreateAssetMenu(fileName = "EnemyDataManager", menuName = "Enemy/EnemyDataManager")]
public class EnemyDataManager : ScriptableObject
{
    [System.Serializable]
    public struct EnemyDataEntry
    {
        public EnemyType enemyType;
        public EnemyDataSO data;
    }

    [SerializeField]
    private List<EnemyDataEntry> enemyDataList;

    private Dictionary<EnemyType, EnemyDataSO> enemyDataDict;

    public void Init()
    {
        if (enemyDataDict == null)
        {
            enemyDataDict = new Dictionary<EnemyType, EnemyDataSO>();
            foreach (var entry in enemyDataList)
            {
                if (!enemyDataDict.ContainsKey(entry.enemyType))
                    enemyDataDict.Add(entry.enemyType, entry.data);
            }
        }
    }

    public EnemyDataSO GetEnemyData(EnemyType type)
    {
        Init(); // 필요 시 초기화
        return enemyDataDict.TryGetValue(type, out var data) ? data : null;
    }
    public List<EnemyDataEntry> GetEnemyDataList() { return enemyDataList; }
}