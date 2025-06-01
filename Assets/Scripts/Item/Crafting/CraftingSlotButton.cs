using UnityEngine;
using UnityEngine.UI;

public class CraftingSlotButton : MonoBehaviour
{
    public CraftItemData recipe;
    public CraftingUIManager uiManager;

    public void OnClick()
    {
        if (!uiManager) return;

        uiManager.SelectSlot(GetComponent<Button>());  // 선택 처리
        uiManager.DisplayRecipe(recipe);               // UI 갱신
    }
}
