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
        if (currentItem == null)
        {
            Debug.LogWarning("사용할 아이템이 없습니다.");
            return;
        }
        // 플레이어 상태에 효과 적용
        Debug.Log($"사용: {currentItem.displayName}");

        var playerCondition = PlayerManager.Instance.condition;
        if (playerCondition == null)
        {
            Debug.LogError("PlayerCondition이 존재하지 않습니다.");
            return;
        }

        // 소비 효과 적용
        foreach (var effect in currentItem.consumables)
        {
            switch (effect.type)
            {
                case ConsumableType.Health:
                    playerCondition.Heal(effect.value);
                    break;
                case ConsumableType.Hunger:
                    playerCondition.RecoverHunger(effect.value);
                    break;
                case ConsumableType.Thirst:
                    playerCondition.RecoverThirst(effect.value);
                    break;
                case ConsumableType.Stamina:
                    playerCondition.GenerateStamina(effect.value);
                    break;
            }

            Debug.Log($"[아이템 효과] {effect.type}: {effect.value}");
        }

        // 인벤토리에서 아이템 1개 소모
        Inventory.Instance.RemoveItem(currentItem, 1);


        // UI 갱신
        InventoryUI ui = FindObjectOfType<InventoryUI>();
        if (ui != null)
        {
            ui.RefreshInventory();
            gameObject.SetActive(false); // 상세창 닫기
        }
    }

    public void OnClickEquip()
    {
        if (currentItem == null)
        {
            Debug.Log("아이템 없음");
            return;
        }
        if (currentItem.type != ItemType.Equipable)
        {
            Debug.Log("장착하는 아이템 아님");
            return;
        }


        var equipmentSystem = PlayerManager.Instance.GetComponent<EquipmentSystem>();
        if (equipmentSystem == null) return;

        var equipped = equipmentSystem.GetEquippedItem(currentItem.equipSlotType);

        if (equipped == currentItem)
        {
            equipmentSystem.UnequipItem(currentItem.equipSlotType);
            Debug.Log($"장비 해제됨: {currentItem.displayName}");
        }
        else
        {
            equipmentSystem.EquipItem(currentItem);
            Debug.Log($"장비 착용됨: {currentItem.displayName}");
        }

        // 필요하다면 UI 갱신
        InventoryUI ui = FindObjectOfType<InventoryUI>();
        if (ui != null)
            ui.RefreshInventory();
    }

    public void OnClickDrop()
    {
        //if (currentItem == null) return;

        // 인벤토리에서 제거
        // Inventory.Instance.RemoveItem(currentItem, 1);
        Debug.Log($"버림: {currentItem.displayName}");
    }
}
