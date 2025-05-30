using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CraftingUIManager : MonoBehaviour
{
    [Header("UI 연결")]
    public TextMeshProUGUI itemNameText;
    public TextMeshProUGUI descriptionText;
    public TextMeshProUGUI weightText;
    public TextMeshProUGUI craftingTimeText;
    public Button craftButton;

    [Header("재료 목록 UI")]
    public Transform requiredInfoParent;
    public GameObject requiredInfoPrefab;

    [Header("팝업 UI")]
    public GameObject craftingResultPopup;
    public TextMeshProUGUI popupText;

    [Header("기본 연결")]
    public Inventory playerInventory;

    [Header("설정")]
    public float defaultCraftTime = 3f;

    private CraftItemData currentRecipe;
    private bool isCrafting = false;

    public void DisplayRecipe(CraftItemData recipe)
    {
        currentRecipe = recipe;

        // 기본 정보 표시
        itemNameText.text = recipe.resultItem.displayName;
        descriptionText.text = recipe.resultItem.description;
        weightText.text = recipe.resultItem.itemMass + " kg";
        craftingTimeText.text = $"{defaultCraftTime} SECONDS";

        // 기존 재료 UI 지우기
        foreach (Transform child in requiredInfoParent)
        {
            Destroy(child.gameObject);
        }

        bool canCraft = true;

        foreach (var req in recipe.materials.Take(4))
        {
            int owned = playerInventory.GetItemCount(req.item);
            if (owned < req.amount) canCraft = false;

            GameObject go = Instantiate(requiredInfoPrefab, requiredInfoParent);
            go.GetComponent<RequiredInfoUI>().Set(req.item.displayName, owned, req.amount);
        }

        craftButton.interactable = canCraft;
    }

    public void OnClickCraft()
    {
        if (!craftButton.interactable || isCrafting) return;

        StartCoroutine(CraftCoroutine());
    }

    IEnumerator CraftCoroutine()
    {
        isCrafting = true;
        craftButton.interactable = false;

        yield return new WaitForSeconds(defaultCraftTime);

        foreach (var req in currentRecipe.materials)
        {
            playerInventory.RemoveItem(req.item, req.amount);
        }

        playerInventory.AddItem(currentRecipe.resultItem, currentRecipe.resultAmount);

        ShowCraftingResultPopup(currentRecipe.resultItem.displayName, currentRecipe.resultAmount);
        DisplayRecipe(currentRecipe); // UI 갱신

        isCrafting = false;
    }

    void ShowCraftingResultPopup(string itemName, int amount)
    {
        popupText.text = $"{itemName} x{amount} 제작 완료!";
        craftingResultPopup.SetActive(true);
        Invoke("HidePopup", 2.5f);
    }

    void HidePopup()
    {
        craftingResultPopup.SetActive(false);
    }
}
