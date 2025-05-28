using UnityEngine;
using UnityEngine.UI;

public class PlayerStatUI : MonoBehaviour
{
    [Header("원형 UI")]
    public Image healthRing;   // 왼쪽 반원
    public Image staminaRing;  // 오른쪽 반원

    [Header("스탯 아이콘")]
    public Image thirstIcon;
    public Image hungerIcon;
    public Image timeIcon;

    [Header("스탯 값 (0~1로 입력)")]
    [Range(0f, 1f)] public float health = 1f;
    [Range(0f, 1f)] public float stamina = 1f;
    [Range(0f, 1f)] public float thirst = 1f;
    [Range(0f, 1f)] public float hunger = 1f;

    private void Update()
    {
        // 반원 테두리 스탯
        healthRing.fillAmount = health;
        staminaRing.fillAmount = stamina;

        // 아이콘 위의 스탯 (작은 원형 게이지처럼 표현하고 싶다면 같은 방식으로 처리 가능)
        thirstIcon.fillAmount = thirst;
        hungerIcon.fillAmount = hunger;

        // 시간 아이콘은 예: 낮/밤 색 변화 or 알파값 변화 등으로 표현 가능
        UpdateTimeIcon();
    }

    private void UpdateTimeIcon()
    {
        float time = TemperatureManager.Instance?.dayNightCycle?.time ?? 0f;

        // 예시: 밤에 아이콘 어둡게 처리
        bool isNight = time < 0.25f || time > 0.75f;
        timeIcon.color = isNight ? Color.gray : Color.white;
    }
}
