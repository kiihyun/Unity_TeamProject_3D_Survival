using UnityEngine;

public class EquipmentSystem : MonoBehaviour
{
    public EquipSlot[] equipSlots; //장착 가능한 아이템들
    public Transform equipPos;

    public void EquipItem(ItemData item)//장착
    {
        foreach (var slot in equipSlots)
        {
            Debug.Log($"[장비 체크] 슬롯 타입: {slot.slotType}, 아이템 타입: {item.equipSlotType}");
            if (slot.slotType == item.equipSlotType)
            {
                slot.equippedItem = item;
                Debug.Log($"착용됨: {item.displayName}");

                GameObject gameObject = Instantiate(slot.equippedItem.equipPrefab, equipPos);
                gameObject.transform.position = equipPos.position;
                return;
            }
        }
        Debug.LogWarning("EquipItem 실패: 일치하는 슬롯을 찾을 수 없습니다.");
    }

    public void UnequipItem(EquipSlotType slotType)//아이템 해제
    {
        foreach (var slot in equipSlots)
        {
            if (slot.slotType == slotType)
            {
                Debug.Log($"해제됨: {slot.equippedItem?.displayName}");
                slot.equippedItem = null;
                return;
            }
        }
    }

    public ItemData GetEquippedItem(EquipSlotType slotType)//장착된 아이템 확인
    {
        foreach (var slot in equipSlots)
        {
            if (slot.slotType == slotType)
                return slot.equippedItem;
        }
        return null;
    }
}