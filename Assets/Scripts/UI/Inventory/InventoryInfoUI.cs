using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class InventoryInfoUI : MonoBehaviour
{
    /// 인벤토리 UI에서 아이템을 선택하면 옆에 상세 정보를 표시하는 UI를 위한 스크립트입니다.

    public static InventoryInfoUI Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }


    public Image iconImage;
    public TMP_Text nameText;
    public TMP_Text descriptionText;
    public TMP_Text quantityText;
    public TMP_Text weightText;
    public TMP_Text conditionText;

    public Button useButton;
    public Button equipButton;
    public Button dropButton;

    private ItemData currentItem;

    // Start is called before the first frame update
    void Start()
    {
        gameObject.SetActive(false);
    }

    public void ShowItemDetail(ItemData item)
    {
        if (item == null)
        {
            Debug.LogError("ShowItemDetail: item is null");
            return;
        }

        Debug.Log($"ShowItemDetail: {item.displayName}");

        currentItem = item;
        gameObject.SetActive(true);

        iconImage.sprite = item.icon;
        nameText.text = item.displayName;
        descriptionText.text = item.description;
        quantityText.text = $"수량: {item.maxStackAmount}";
        weightText.text = $"무게: {item.itemMass} kg";

        if (item.type == ItemType.Equipable)
            conditionText.text = $"내구도: {item.durability}%";
        else
            conditionText.text = "";

        // 버튼 활성화 여부
        useButton.gameObject.SetActive(item.type == ItemType.Consumable);
        equipButton.gameObject.SetActive(item.type == ItemType.Equipable);
        dropButton.gameObject.SetActive(true);


    }

    public void OnClickUse()
    {
        if (currentItem == null) return;

        // 플레이어 상태에 효과 적용
        // ex: PlayerStatus.Instance.ApplyConsumable(currentItem);
        Debug.Log($"사용: {currentItem.displayName}");
    }

    public void OnClickDrop()
    {
        if (currentItem == null) return;

        // 인벤토리에서 제거
        // Inventory.Instance.RemoveItem(currentItem, 1);
        Debug.Log($"버림: {currentItem.displayName}");
    }
}
