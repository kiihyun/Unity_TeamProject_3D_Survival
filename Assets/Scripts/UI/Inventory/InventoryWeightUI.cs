using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventoryWeightUI : MonoBehaviour
{
    private PlayerCondition playerCondition;

    [SerializeField] private Slider weightSlider;
    [SerializeField] private TextMeshProUGUI weightText;


    private float maxWeight = 50;
    private float curWeight;

    private void Awake()
    {
        // PlayerCondition 컴포넌트를 씬에서 찾아 할당 (예: 플레이어 오브젝트에 붙어있다고 가정)
        playerCondition = FindObjectOfType<PlayerCondition>();
        if (playerCondition == null)
        {
            Debug.LogError("PlayerCondition 컴포넌트를 찾을 수 없습니다.");
        }
        else
        {
            Debug.Log("PlayerCondition 컴포넌트 할당 완료");
        }

        if (weightSlider == null)
            Debug.LogError("weightSlider가 할당되지 않았습니다.");
        if (weightText == null)
            Debug.LogError("weightText가 할당되지 않았습니다.");
    }

    private void Start()
    {
        Inventory.Instance.onInventoryChanged += () =>
        {
            float weight = PlayerManager.Instance.condition.currentCarryWeight;
            UpdateWeightDisplay(weight);
        };

        // 시작 시에도 한번 출력
        UpdateWeightDisplay(PlayerManager.Instance.condition.currentCarryWeight);

    }

    public void UpdateWeightDisplay(float curWeight)
    {
        Debug.Log($"현재 무게: {curWeight}, 최대 무게: {maxWeight}");

        weightSlider.maxValue = maxWeight;
        weightSlider.value = curWeight;
        weightText.text = $"{curWeight} / {maxWeight}KG";
    }
}
