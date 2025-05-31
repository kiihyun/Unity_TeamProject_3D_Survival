using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;

public class Tree : MonoBehaviour
{

    public ItemData itemData;
    public int amount = 1;
    public int count = 0;
    public int maxCount = 3;
    public float cycle;
    // 플레이어가 호출함

    private void Update()
    {
        if (count > maxCount)
        {
            cycle += Time.deltaTime;
            if (cycle > 20f)
            {
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
