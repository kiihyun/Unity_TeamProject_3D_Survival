using UnityEngine;

public enum WeatherType
{
    Clear,
    Cloudy,
    Rain,
    Snow,
    Storm,
    Foggy
}

public class WeatherManager : MonoBehaviour
{
    public static WeatherManager Instance { get; private set; }

    public WeatherType currentWeather { get; private set; }

    [Header("Weather Prefabs")]
    public GameObject rainEffect;
    public GameObject snowEffect;
    public GameObject stormEffect;
    public GameObject fogEffect;

    private GameObject currentEffect;

    private void Awake()
    {
        if (Instance != null && Instance != this) Destroy(gameObject);
        else Instance = this;
    }

    private void Start()
    {
        SetWeather(WeatherType.Clear);
    }

    public void SetWeather(WeatherType newWeather)
    {
        if (currentEffect != null) Destroy(currentEffect);

        currentWeather = newWeather;
        switch (newWeather)
        {
            case WeatherType.Clear:
                break;

            case WeatherType.Cloudy:
                // 구름 이미지 오버레이 등
                break;

            case WeatherType.Rain:
                currentEffect = Instantiate(rainEffect, transform);
                break;

            case WeatherType.Snow:
                currentEffect = Instantiate(snowEffect, transform);
                break;

            case WeatherType.Storm:
                currentEffect = Instantiate(stormEffect, transform);
                // 번개나 바람 소리 추가 가능
                break;

            case WeatherType.Foggy:
                currentEffect = Instantiate(fogEffect, transform);
                RenderSettings.fog = true;
                break;
        }

        if (newWeather != WeatherType.Foggy)
        {
            RenderSettings.fog = false;
        }

        Debug.Log($"[WeatherManager] 현재 날씨: {newWeather}");
    }
}
