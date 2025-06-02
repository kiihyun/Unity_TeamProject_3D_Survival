using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WeightUI : MonoBehaviour
{
    //캐릭터의 인벤토리 속 아이템의 총 무게를 계산해서 UI에 표시해주는 스크립트입니다.
    [SerializeField] private Slider weightSlider;
    public TextMeshProUGUI weightText;
    public TextMeshProUGUI weightStateText;

    void Update()
    {
        var condition = PlayerManager.Instance.condition;

        weightText.text = $"{condition.currentCarryWeight:F1} / {condition.maxCarryWeight}";
        weightStateText.text = GetWeightStatusString(condition.WeightStatus);
    }

    private string GetWeightStatusString(WeightState state)
    {
        switch (state)
        {
            case WeightState.Light: return "<color=green>가벼움</color>";
            case WeightState.Normal: return "<color=white>보통</color>";
            case WeightState.Heavy: return "<color=orange>무거움</color>";
            case WeightState.Overloaded: return "<color=red><b>아 너무 무겁다</b></color>";
            default: return "";
        }
    }
}