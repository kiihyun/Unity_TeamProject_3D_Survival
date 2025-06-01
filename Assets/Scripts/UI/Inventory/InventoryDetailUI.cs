using TMPro;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Progress;

public class InventoryDetailUI : MonoBehaviour
{
    public static InventoryDetailUI Instance { get; private set; }

    public TextMeshProUGUI itemNameText;
    public TextMeshProUGUI itemDescText;
    public TextMeshProUGUI itemMassText;
    public TextMeshProUGUI itemQuenText;

    public GameObject useButton;
    public GameObject equipButton;
    public GameObject dropButton;



    private ItemData currentItem;

    private void Awake()
    {
        Instance = this;
    }


    public void ShowItemDetail(ItemData data)
    {

        if (data == null)
        {
            Debug.LogError("ShowItemDetail: item is null");
            return;
        }
        Debug.Log($"ShowItemDetail: {data.displayName}");

        currentItem = data;
        itemNameText.text = data.displayName;
        itemDescText.text = data.description;
        itemMassText.text = $"{data.itemMass.ToString()}kg";
        itemQuenText.text = Inventory.Instance.GetItemCount(data).ToString();


        // 버튼 활성화 여부
        ////아이템 데이터가 사용 가능한 경우
        useButton.gameObject.SetActive(data.type == ItemType.Consumable);
        ////아이템 데이터가 장착 가능한 경우
        equipButton.gameObject.SetActive(data.type == ItemType.Equipable);
        dropButton.gameObject.SetActive(true);
    }

    public void OnClickUse() {
        if (currentItem == null)
        {
            Debug.LogWarning("사용할 아이템이 없습니다.");
            return;
        }

        // 플레이어 상태에 효과 적용
        Debug.Log($"사용: {currentItem.displayName}");

        if (currentItem.canPlace)
        {
            PlayerManager.Instance.placeSystem.StartPlacing(currentItem);
            gameObject.SetActive(false);
            return;
        }

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

        /* 아이템 사용 처리 */
    }
    public void OnClickEquip() {

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


        var equipmentSystem = PlayerManager.Instance.player.GetComponent<EquipmentSystem>();
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
        /* 장착 처리 */
    }
    public void OnClickDrop() {
        Inventory.Instance.RemoveItem(currentItem, 1);
        Debug.Log($"버림: {currentItem.displayName}");/* 드롭 처리 */
    }
}
