using UnityEngine;

/// <summary>
/// 낮과 밤의 주기를 시뮬레이션하는 스크립트.
/// - 시간(time)은 0~1 범위로 하루를 표현함 (0 = 자정, 0.25 = 오전 6시, 0.5 = 정오, 0.75 = 오후 6시)
/// - 태양과 달의 위치, 색상, 밝기를 시간에 따라 조정함
/// - ambient/reflection 조명도 시간 기반으로 변화함
/// - temperatureCurve를 통해 시간에 따른 월드 온도도 함께 전달함
/// </summary>
public class DayNightCycle : MonoBehaviour
{
    [Range(0.0f, 1.0f)]
    public float time; // 하루 중 현재 시간 (0~1)

    public float fullDayLength = 120f; // 한 '가상 하루'가 실제 몇 초에 해당하는지
    public float startTime = 0.4f;     // 게임 시작 시 시간 (0.4 = 오전 9시 36분쯤)

    private float timeRate;           // 하루의 시간 단위가 얼마나 빠르게 흐르는지를 나타냄 (1 / fullDayLength)
    public Vector3 noon = new Vector3(0f, 360f, 0f); // 정오 기준 광원 회전값

    [Header("Sun")]
    public Light sun;                     // 태양 광원
    public Gradient sunColor;            // 시간에 따른 태양 색상 변화
    public AnimationCurve sunIntensity;  // 시간에 따른 태양 밝기 변화

    [Header("Moon")]
    public Light moon;                   // 달 광원
    public Gradient moonColor;           // 시간에 따른 달 색상 변화
    public AnimationCurve moonIntensity; // 시간에 따른 달 밝기 변화

    [Header("Other Lighting")]
    public AnimationCurve lightingIntensityMultiplier;     // 시간에 따른 환경광 밝기 변화
    public AnimationCurve reflectionIntensityMultiplier;   // 시간에 따른 반사광 밝기 변화

    [Header("Weather Settings")]
    public float weatherChangeInterval = 0.25f; // 하루 중 날씨 변경 간격 (0.25 = 6시간)
    private float lastWeatherCheckTime = -1f;

    private void Start()
    {
        // 하루가 얼마나 빠르게 흐를지 계산 (ex. 120초 = 1일이면 1/120만큼씩 증가)
        timeRate = (fullDayLength > 0f) ? 1.0f / fullDayLength : 0f;

        // 시작 시간 설정
        time = startTime % 1.0f;
    }

    private void Update()
    {
        // 하루 시간 경과 처리 (루프: 0.0 ~ 1.0)
        time = (time + timeRate * Time.deltaTime) % 1.0f;

        // 태양과 달의 상태 업데이트
        UpdateLighting(sun, sunColor, sunIntensity);
        UpdateLighting(moon, moonColor, moonIntensity);

        // 전역 조명 반영
        RenderSettings.ambientIntensity = lightingIntensityMultiplier.Evaluate(time);
        RenderSettings.reflectionIntensity = reflectionIntensityMultiplier.Evaluate(time);

        // 날씨 업데이트
        UpdateWeather();
    }

    /// <summary>
    /// 주어진 광원(태양 또는 달)에 대해 회전, 색상, 밝기를 업데이트하고
    /// 일정 밝기 이하일 땐 비활성화 처리
    /// </summary>
    void UpdateLighting(Light lightSource, Gradient colorGradient, AnimationCurve intensityCurve)
    {
        float intensity = intensityCurve.Evaluate(time); // 현재 시간 기준 밝기

        // 광원 회전 (정오 기준 방향 * 시간 * 4 → 하루 1회전)
        float timeOffset = (lightSource == sun) ? 0.25f : 0.75f;
        lightSource.transform.eulerAngles = (time - timeOffset) * noon * 4.0f;

        // 광원 색상 및 밝기 적용
        lightSource.color = colorGradient.Evaluate(time);
        lightSource.intensity = intensity;

        // 밝기가 0이면 비활성화하여 최적화
        GameObject go = lightSource.gameObject;
        bool shouldBeActive = intensity > 0f;
        if (go.activeSelf != shouldBeActive)
            go.SetActive(shouldBeActive);
    }

    private void UpdateWeather()
    {
        // 일정 시간대마다 한 번만 날씨 변경 시도 
        float currentChunk = Mathf.Floor(time / weatherChangeInterval) * weatherChangeInterval;
        if (Mathf.Approximately(currentChunk, lastWeatherCheckTime)) return;

        lastWeatherCheckTime = currentChunk;

        // 모든 날씨 타입 중에서 무작위 선택
        WeatherType[] allWeathers = (WeatherType[])System.Enum.GetValues(typeof(WeatherType));
        WeatherType newWeather = allWeathers[Random.Range(0, allWeathers.Length)];

        WeatherManager.Instance.SetWeather(newWeather);

        // 현재 날씨 디버그 출력
        Debug.Log($"[DayNightCycle] {System.DateTime.Now:T} - 날씨 변경됨 → {newWeather}");
    }
}