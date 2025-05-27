using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlotUI : MonoBehaviour
{
    private ItemData item;

    public Button button;
    public Image icon;
    public TextMeshProUGUI quantityText;
    public TextMeshProUGUI weightText;
    private Outline outline;

    public int index;
    public bool equipped;
    public int quantity;
    
    private void Awake()
    {
        outline = GetComponent<Outline>();
        //button.onClick.AddListener(OnClick);
    }

    public void SetItem(ItemData itemData, int quantity = 1)
    {
        icon.gameObject.SetActive(true);
        icon.sprite = item.icon;
        quantityText.text = quantity > 1 ? quantity.ToString() : string.Empty;
        
        if (outline != null)
        {
            outline.enabled = equipped;
        }
    }
    public void ClearItem()
    {
        item = null;
        icon.gameObject.SetActive(false);
        quantityText.text = string.Empty;
    }

    public void OnClickButton()
    {
        FindObjectOfType<InventoryUI>().OnItemClicked(item);
    }
}
