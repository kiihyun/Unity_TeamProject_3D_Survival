using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    public Inventory inventory;// 플레이어의 인벤토리 받아와서 UI에 표시

    [SerializeField] private Transform slotContainer;
    [SerializeField] private GameObject slotPrefab;

    private void Start()
    {
        // 인벤토리가 변경될 때마다 UI 새로고침
        inventory.onInventoryChanged += RefreshInventory;
        RefreshInventory();
    }

    public void RefreshInventory()//안에 List<ItemData> items 안 넣어도 될 거 같습니다.
    {
        // 슬롯 초기화 후 재생성
        foreach (Transform child in slotContainer)
            Destroy(child.gameObject);

        // 현재 인벤토리 슬롯만큼 새 슬롯 오브젝트 생성
        foreach (var slot in inventory.slots)
        {
            GameObject slotGO = Instantiate(slotPrefab, slotContainer);

            // 텍스트 찾아서 아이템 이름 + 수량 표시
            var text = slotGO.GetComponentInChildren<TextMeshProUGUI>();
            if (text != null)
                text.text = $"{slot.item.displayName} x{slot.count}";
        }
    }

    public void OnItemClicked(ItemData itemData)
    {
        InventoryDetailUI.Instance.ShowItemDetail(itemData);
    }
}
