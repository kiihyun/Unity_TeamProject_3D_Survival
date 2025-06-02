using TMPro;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Progress;

public class CraftingDetailUI : MonoBehaviour
{
    public static CraftingDetailUI Instance { get; private set; }

    public TextMeshProUGUI itemNameText;
    public TextMeshProUGUI itemDescText;

    private ItemData currentItem;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        // 초기화
        itemNameText.text = string.Empty;
        itemDescText.text = string.Empty;
    }

    public void ShowItemDetail(ItemData data)
    {

        if (data == null)
        {
            Debug.LogError("ShowItemDetail: item is null");
            return;
        }
        Debug.Log($"ShowItemDetail: {data.displayName}");

        currentItem = data;
        itemNameText.text = data.displayName;
        itemDescText.text = data.description;
    }
}
