using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolManager : Singleton<ObjectPoolManager>
{
    private Dictionary<int, List<GameObject>> dics  = new Dictionary<int, List<GameObject>>();
    public GameObject GetObjectByPrefab(GameObject prefab,Transform parent, Vector3 position)
    {
        int hashCode = prefab.GetHashCode();

        if (!dics.ContainsKey(hashCode))
        {
            dics.Add(hashCode, new List<GameObject>());
        }

        List<GameObject> gameObjects = dics[hashCode];

        foreach (var item in gameObjects)
        {
            if (!item.gameObject.activeInHierarchy)
            {
                item.transform.SetParent(parent);
                item.transform.localPosition = position;
                item.gameObject.SetActive(true);
                return item;
            }
        }

        GameObject obj = Instantiate(prefab, parent);
        obj.transform.localPosition = position;
        dics[hashCode].Add(obj);
        return obj;
    }
}
