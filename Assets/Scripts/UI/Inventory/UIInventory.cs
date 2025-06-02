using UnityEngine;
using TMPro;

public class UIInventory : MonoBehaviour
{
    //인벤토리 데이터를 기반으로 UI 슬롯을 생성
    public Inventory inventory;//플레이어 인벤토리 가져옴
    public Transform slotParent;//슬롯이 들어갈 부모 오브젝트 <- 그리드 레이아웃 그룹 붙어있는 곳
    public GameObject slotPrefab;//슬롯에 표시할 아이콘과 텍스트 프리팹

    private void Start()
    {
        // 인벤토리가 변경될 때마다 UI 새로고침
        inventory.onInventoryChanged += RefreshUI;
        RefreshUI();
    }

    void RefreshUI()
    {
        //새로고침해서 현재 인벤토리 상태를 화면에 표시
        foreach (Transform child in slotParent)
            Destroy(child.gameObject);
        //인벤토리에 있는 아이템 종류만큼 슬롯 생성
        foreach (var slot in inventory.slots)
        {
            GameObject slotGO = Instantiate(slotPrefab, slotParent);
            // 텍스트 컴포넌트를 찾아서 아이템 이름 + 수량 표시
            var text = slotGO.GetComponentInChildren<TextMeshProUGUI>();
            if (text != null)
                text.text = $"{slot.item.displayName} x{slot.count}";
        }
    }
}
