using UnityEngine;
using UnityEngine.TextCore.Text;


public interface IItemInteractable
{
    public string GetInteractPrompt();
    public void OnInteract();
}


public class ItemObject : MonoBehaviour, IItemInteractable
{
    public ItemData data;

    public string GetInteractPrompt()
    {
        string str = $"{data.displayName}";
        return str;
    }

    public void OnInteract()
    {
        //CharacterManager.Instance.Player.itemData = data;
        //CharacterManager.Instance.Player.addItem?.Invoke();
        Inventory.Instance.AddItem(data);
        Destroy(gameObject);

    }
}
