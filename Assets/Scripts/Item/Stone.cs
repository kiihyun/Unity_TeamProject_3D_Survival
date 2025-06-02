using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Stone : MonoBehaviour
{
    public GameObject resourceObject;
    private BoxCollider collider;
    public ItemData itemData;
    private bool isPicked = false;
    public int amount = 1;
    public float cycle;
    public float maxCycle = 20f;
    // 플레이어가 호출함

    private void Start()
    {
        collider = GetComponent<BoxCollider>();
    }
    private void Update()
    {
        if (isPicked) 
        {
            resourceObject.SetActive(false);
            collider.enabled = false;
            cycle += Time.deltaTime;
            if (cycle > maxCycle)
            {
                resourceObject.SetActive(true);
                collider.enabled = true;
                cycle = 0;
                isPicked = false;
            }
        }
    }

    public void ItemInteract(Inventory inventory)
    {
        isPicked = true;
        inventory.AddItem(itemData, amount);
        Debug.Log($"{itemData.displayName} 얻음");
    }
}
