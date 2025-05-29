using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EnemyDataEntry
{
    public EnemyDataSO data;
    [HideInInspector] public List<GameObject> activeEnemies = new();
}

