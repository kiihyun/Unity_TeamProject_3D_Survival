using UnityEngine;
using static UnityEditor.Progress;

public class InventoryUI : MonoBehaviour
{


    // 플레이어 인벤토리 데이터를 받아와서 UI 슬롯들을 표시하고, 슬롯 프리팹에 드래그 가능한 UI를 생성하는 스크립트

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

            InventorySlotUI slotUI = slotGO.GetComponent<InventorySlotUI>();
            if (slotUI != null)
            {
                slotUI.index = slot.index;
                slotUI.SetItem(slot.item, slot.count);
            }

            /// 텍스트 찾아서 아이템 이름 + 수량 표시
            //var text = slotGO.GetComponentInChildren<TextMeshProUGUI>();
            //if (text != null)
            //    text.text = $"{slot.item.displayName} x{slot.count}";
        }


        //// 현재 인벤토리 슬롯마다 새로운 UI 슬롯 생성
        //for (int i = 0; i < inventory.slots.Count; i++)
        //{
        //    var slotData = inventory.slots[i];

        //    GameObject slotGO = Instantiate(slotPrefab, slotContainer);
        //    var slotUI = slotGO.GetComponent<InventorySlotUI>();

        //    if (slotUI != null)
        //    {
        //        slotUI.index = i;
        //        slotUI.SetItem(slotData.item, slotData.count);
        //    }
        //    else
        //    {
        //        // 예외 처리: 만약 Text만 있고 InventorySlotUI가 없다면 텍스트만 채움
        //        var text = slotGO.GetComponentInChildren<TextMeshProUGUI>();
        //        if (text != null)
        //            text.text = $"{slotData.item.displayName} x{slotData.count}";
        //    }
        //}


    }

    public void OnItemClicked(ItemData itemData)
    {

        //InventoryInfoUI.Instance.ShowItemDetail(itemData);
        //만약 아래거 쓰고 싶으면 위에거 비활성화 시키고 쓰기
        InventoryDetailUI.Instance.ShowItemDetail(itemData);
        if (itemData == null)
        {
            Debug.LogError("클릭된 슬롯의 item이 null입니다.");
            return;
        }

        if (InventoryInfoUI.Instance == null)
        {
            Debug.LogError("InventoryInfoUI.Instance가 null입니다.");
            return;
        }

    }
}
