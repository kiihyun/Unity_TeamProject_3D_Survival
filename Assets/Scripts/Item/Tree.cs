using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;

public class Tree : MonoBehaviour
{
    public GameObject treeObject;
    private BoxCollider collider;
    public ItemData itemData;
    public int amount = 1;
    public int count = 0;
    public int maxCount = 3;
    public float cycle;
    public float maxCycle = 20f;
    // 플레이어가 호출함

    private void Start()
    {
        collider = GetComponent<BoxCollider>();
    }
    private void Update()
    {
        if (count >= maxCount)
        {
            treeObject.SetActive(false);
            collider.enabled = false;
            cycle += Time.deltaTime;
            if (cycle > maxCycle)
            {
                treeObject.SetActive(true);
                collider.enabled = true;
                cycle = 0;
                count = 0;
            }

        }
    }

    public void ItemInteract(Inventory inventory)
    {
        count++;
        if (count <= maxCount)
        {
            inventory.AddItem(itemData, amount);
            Debug.Log($"{itemData.displayName} 얻음");
        }
    }
}
