using UnityEngine;

public class InventoryHotkey : MonoBehaviour
{
    //단축키로 아이템 사용 혹은 장착하기 위한 스크립트
    public Inventory inventory;
    public PlayerCondition playerCondition;

    public ShortcutManager shortcutManager;

    // Update is called once per frame
    void Update()
    {
        // 숫자키 1~9 감지 (Alpha1은 1번 슬롯, Alpha9은 9번 슬롯)
        for (int i = 0; i < 9; i++)
        {
            // 사용자가 숫자키 입력 시
            if (Input.GetKeyDown(KeyCode.Alpha1 + i))
            {
                // 해당 슬롯 인덱스의 아이템 가져오기
                ItemData item = shortcutManager.GetShortcut(i);
                if (item == null) continue;

                // 아이템 타입에 따라 행동 결정
                if (item.type == ItemType.Consumable && item.consumables.Length > 0)
                {
                    // 소비형 아이템인 경우 효과 적용
                    foreach (var effect in item.consumables)
                    {
                        ApplyConsumableEffect(effect);
                    }

                    // 아이템 한 개 소모
                    inventory.RemoveItem(item, 1); // 인벤토리에서 제거
                }

                if (item.type == ItemType.Equipable)
                {
                    //아이템이 무기 같은 장착형이라면
                    //이미 장착 중이라면 장착 해제
                    //장착 중이 아니라면 장착
                }
            }
        }
    }

    /// 숏컷에 있는 게 소비형 아이템이라면 플레이어한테 적용 되도록 하는 함수
    void ApplyConsumableEffect(ItemDataConsumable effect)
    {
        switch (effect.type)
        {
            case ConsumableType.Health:
                playerCondition.Heal(effect.value);
                break;
            case ConsumableType.Hunger:
                playerCondition.Hunger += effect.value;
                break;
            case ConsumableType.Stamina:
                playerCondition.Stamina += effect.value;
                break;
            case ConsumableType.Thirst:
                playerCondition.Thirst += effect.value;
                break;
        }
    }
}
