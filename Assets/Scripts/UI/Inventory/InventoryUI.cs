using System.Collections.Generic;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    [SerializeField] private Transform slotContainer;
    [SerializeField] private GameObject slotPrefab;

    public void RefreshInventory(List<ItemData> items)
    {
        // 슬롯 초기화 후 재생성
    }

    public void OnItemClicked(ItemData itemData)
    {
        InventoryDetailUI.Instance.ShowItemDetail(itemData);
    }
}
