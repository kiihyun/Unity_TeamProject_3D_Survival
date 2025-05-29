using UnityEngine;
public enum ItemType
{
    Equipable,
    Consumable,
    Resource,
    ForEvent
}

public enum EquipSlotType
{
    Weapon,
    Armor,
    Accessory
}
[System.Serializable]
public class EquipSlot
{
    public EquipSlotType slotType;
    public ItemData equippedItem;
}

public enum ConsumableType
{
    Health,
    Hunger,
    Thirst,
    Stamina
}

[System.Serializable]
public class ItemDataConsumable
{
    public ConsumableType type;
    public int value;
}

[CreateAssetMenu(fileName = "Item", menuName = "New Item")]
public class ItemData : ScriptableObject
{
    [Header("INFO")]
    public string displayName;
    public string description;
    public float itemMass;
    public ItemType type;
    public Sprite icon;
    public GameObject dropPrefab;

    [Header("Stacking")]
    public bool canStack;
    public int maxStackAmount;

    [Header("Consumable")]
    public ItemDataConsumable[] consumables;

    [Header("Equip")]
    public EquipSlotType equipSlotType;
    public GameObject equipPrefab;
    public int durability;
    public bool canPlace;
}

