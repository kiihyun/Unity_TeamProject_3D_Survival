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
    //여기 부분은 다양하게 설정 가능(주위에 불이 있어야 한다, 선행 조건이 있어야 한다 등등)
    public bool requiresNearFire = false;
    public string requiredCondition; // 예: 퀘스트를 완료해야 만들 수 있다.라고 조건을 만들고 싶다면 "FinishedQuest1"를 확인하도록

}

[System.Serializable]
public class MaterialRequirement
{
    public ItemData item;    // 필요한 재료
    public int amount;       // 재료 수량
}
