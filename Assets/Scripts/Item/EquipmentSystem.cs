using UnityEngine;

public class EquipmentSystem : MonoBehaviour
{
    public EquipSlot[] equipSlots; //장착 가능한 아이템들

    public void EquipItem(ItemData item)//장착
    {
        foreach (var slot in equipSlots)
        {
            if (slot.slotType == item.equipSlotType)
            {
                slot.equippedItem = item;
                Debug.Log($"착용됨: {item.displayName}");
                return;
            }
        }
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