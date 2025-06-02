using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    //플레이어에게 붙이는 싱글턴 컴포넌트입니다.

    public static Inventory Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    public List<InventorySlot> slots = new();
    public int maxSlots = 20; //최대 슬롯 개수



    // 인벤토리가 변경될 때 호출되는 이벤트 (UI 갱신용)
    public delegate void OnInventoryChanged();
    public event OnInventoryChanged onInventoryChanged;



    public bool AddItem(ItemData item, int amount = 1)
    {
        //인벤토리 무게 계산, 만약 초과한다면 아이템 획득 불가능
        float newWeight = GetTotalWeight() + item.itemMass * amount;
        if (newWeight > PlayerManager.Instance.condition.maxCarryWeight)
        {
            Debug.Log("무게 초과로 아이템을 획득할 수 없습니다.");
            return false;
        }

        // 스택 가능한 경우 + 기존 슬롯에 이미 존재할 경우 숫자 추가
        InventorySlot slot = slots.Find(s => s.item == item && item.canStack);
        if (slot != null)
        {
            slot.count += amount;
        }
        else
        {
            if (slots.Count >= maxSlots)
            {
                Debug.Log("인벤토리 초과로 아이템을 획득할 수 없습니다.");
                return false;
            } // 인벤토리 꽉 참
            slots.Add(new InventorySlot(item, amount));
        }

        onInventoryChanged?.Invoke();

        PlayerManager.Instance.condition.currentCarryWeight += item.itemMass * amount;

        return true;
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
        PlayerManager.Instance.condition.currentCarryWeight -= item.itemMass * amount;
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



    ///인벤토리 총 아이템의 무게를 계산하는 함수
    ///아이템 추가하거나 제거할 때마다 이 함수 호출해주기!!
    public float GetTotalWeight()
    {
        float total = 0f;
        foreach (var slot in slots)
        {
            if (slot.item != null)
                total += slot.item.itemMass * slot.count;
        }
        return total;
    }


}
