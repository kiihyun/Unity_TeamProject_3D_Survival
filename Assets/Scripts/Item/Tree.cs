using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tree : MonoBehaviour
{
    
    public ItemData itemData;
    public int amount = 1;
    // 플레이어가 호출함
    public void ItemInteract(Inventory inventory)
    {

        inventory.AddItem(itemData, amount);
        Debug.Log($"{itemData.displayName} 얻음");
    }
}
