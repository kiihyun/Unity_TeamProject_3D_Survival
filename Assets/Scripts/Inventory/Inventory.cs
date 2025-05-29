using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    //플레이어에게 붙이는 컴포넌트입니다.
    // Start is called before the first frame update
    public List<InventorySlot> slots = new();
    public int maxSlots = 20; //최대 슬롯 개수



    // 인벤토리가 변경될 때 호출되는 이벤트 (UI 갱신용)
    public delegate void OnInventoryChanged();
    public event OnInventoryChanged onInventoryChanged;



    public void AddItem(ItemData item, int amount = 1)
    {
        // 스택 가능한 경우 + 기존 슬롯에 이미 존재할 경우 숫자 추가
        InventorySlot slot = slots.Find(s => s.item == item && item.canStack);
        if (slot != null)
        {
            slot.count += amount;
        }
        else
        {
            if (slots.Count >= maxSlots) return; // 인벤토리 꽉 참
            slots.Add(new InventorySlot(item, amount));
        }

        onInventoryChanged?.Invoke();
    }


    //아이템 빼기
    public void RemoveItem(ItemData item, int amount = 1)
    {
        InventorySlot slot = slots.Find(s => s.item == item);
        if (slot == null) return;

        slot.count -= amount;
        if (slot.count <= 0)
            slots.Remove(slot);

        onInventoryChanged?.Invoke();
    }

    //특정 아이템이 일정 수량 이상 있는지 확인합니다. 디폴트 1
    public bool HasItem(ItemData item, int amount = 1)
    {
        InventorySlot slot = slots.Find(s => s.item == item);
        return slot != null && slot.count >= amount;
    }
    //인덱스를 기반으로 슬롯을 가져오기
    public InventorySlot GetSlotByIndex(int index)
    {
        return (index >= 0 && index < slots.Count) ? slots[index] : null;
    }
    //제작할 때 사용될 재료의 정확한 갯수 확인.(ex. 나무 2/5, 3개 부족하다는 뜻)
    public int GetItemCount(ItemData item)
    {
        InventorySlot slot = slots.Find(s => s.item == item);
        return slot != null ? slot.count : 0;
    }
}
