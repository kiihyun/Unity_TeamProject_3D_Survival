using UnityEngine;

[CreateAssetMenu(fileName = "CraftRecipe", menuName = "Crafting/Recipe")]
public class CraftItemData : ScriptableObject
{
    [Header("Output")]
    public ItemData resultItem;      // 만들어지는 아이템
    public int resultAmount = 1;     // 만들어지는 수량

    [Header("Required Materials")]
    public MaterialRequirement[] materials;

    [Header("Craft Conditions")]
    //여기 부분은 다양하게 설정 가능(주위에 강이 있어야 한다, 선행 조건이 있어야 한다 등등)
    public bool requiresNearFire = false;

}

[System.Serializable]
public class MaterialRequirement
{
    public ItemData item;    // 필요한 재료
    public int amount;       // 재료 수량
}
