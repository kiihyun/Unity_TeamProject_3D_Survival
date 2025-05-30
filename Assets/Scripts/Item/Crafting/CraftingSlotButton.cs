using UnityEngine;

public class CraftingSlotButton : MonoBehaviour
{
    public CraftItemData recipe;
    public CraftingUIManager uiManager;

    public void OnClick()
    {
        uiManager.DisplayRecipe(recipe);
    }
}
