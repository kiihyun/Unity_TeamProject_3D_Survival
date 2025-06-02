using UnityEngine;

public class Corpse : MonoBehaviour
{
    public ItemData lootItem;
    public int amount = 1;

    private bool hasLooted = false;

    public void Interact(Inventory inventory)
    {
        if (hasLooted) return;

        bool added = inventory.AddItem(lootItem, amount);
        if (added)
        {
            hasLooted = true;

            Debug.Log("시체에서 아이템을 루팅했습니다.");
        }
    }
}