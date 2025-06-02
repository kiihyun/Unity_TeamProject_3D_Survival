using UnityEngine;

[System.Serializable]
public class InventorySlot
{
    public ItemData item;
    public int count;
    public int index;

    public InventorySlot(ItemData item, int count, int index = -1)
    {
        this.item = item;
        this.count = count;
        this.index = index;
    }
}
