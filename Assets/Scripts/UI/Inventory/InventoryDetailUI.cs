using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryDetailUI : MonoBehaviour
{
    public static InventoryDetailUI Instance { get; private set; }

    public TextMeshProUGUI itemNameText;
    public TextMeshProUGUI itemDescText;

    public GameObject useButton;
    public GameObject equipButton;
    public GameObject dropButton;

    private ItemData currentItem;

    public void ShowItemDetail(ItemData data)
    {
        currentItem = data;
        itemNameText.text = data.displayName;
        itemDescText.text = data.description;

        ////아이템 데이터가 사용 가능한 경우
        //useButton.gameObject.SetActive(data.type.Consumable);
        ////아이템 데이터가 장착 가능한 경우
        //equipButton.gameObject.SetActive(data.isEquipable);
    }

    public void OnClickUse() { /* 아이템 사용 처리 */ }
    public void OnClickEquip() { /* 장착 처리 */ }
    public void OnClickDrop() { /* 드롭 처리 */ }
}
