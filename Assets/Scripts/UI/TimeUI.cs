using UnityEngine;
using UnityEngine.UI;

public class TimeUI : MonoBehaviour
{
    public Image timeGauge; // 원형 게이지 이미지
    public DayNightCycle dayNightCycle;
    void Update()
    {
        if (dayNightCycle != null && timeGauge != null)
        {
            timeGauge.fillAmount = dayNightCycle.time;
        }
    }
}
