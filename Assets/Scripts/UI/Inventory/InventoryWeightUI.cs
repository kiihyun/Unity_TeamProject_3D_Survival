using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventoryWeightUI : MonoBehaviour
{
    [SerializeField] private Slider weightSlider;
    [SerializeField] private TextMeshProUGUI weightText;

    private float maxWeight = 50f;

    public void UpdateWeightDisplay(float currentWeight)
    {
        weightSlider.maxValue = maxWeight;
        weightSlider.value = currentWeight;
        weightText.text = $"{currentWeight} / {maxWeight}KG";
    }

    public void SetMaxWeight(float newMax)
    {
        maxWeight = newMax;
        weightSlider.maxValue = maxWeight;
    }
}
