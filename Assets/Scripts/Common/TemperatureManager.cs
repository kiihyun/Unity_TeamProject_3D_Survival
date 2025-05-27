using UnityEngine;

/// <summary>
/// 게임 내 전체 온도를 관리하는 매니저.
/// - 낮/밤 주기에 따라 변하는 "월드 온도"를 DayNightCycle로부터 받음.
/// - 플레이어의 체온을 환경 온도에 따라 자동으로 변화시킴.
/// - 저체온증/고열 여부도 판정 가능.
/// 
/// 싱글턴 패턴으로 구성되어 어느 스크립트든 TemperatureManager.Instance를 통해 접근 가능.
/// </summary>
public class TemperatureManager : MonoBehaviour
{
    // 싱글턴 인스턴스 (외부에서 TemperatureManager.Instance로 접근 가능)
    public static TemperatureManager Instance { get; private set; }

    [Tooltip("시간에 따른 월드 온도 곡선 (x: 0~1 시간, y: 온도 °C)")]
    public AnimationCurve temperatureCurve;

    [Tooltip("현재 온도 (읽기 전용)")]
    public float currentTemperature { get; private set; }

    [Tooltip("DayNightCycle 스크립트 참조")]
    public DayNightCycle dayNightCycle;

    [Header("World Temperature")]
    [SerializeField]
    private float worldTemperature; // 현재 월드의 실제 온도 (DayNightCycle에서 설정함)

    [Header("Player Body")]
    public float playerBodyTemperature = 36.5f; // 플레이어 현재 체온 (기본값 36.5도)
    public float normalBodyTemperature = 36.5f; // 기준 정상 체온

    public float minBodyTemperature = 30f; // 최저 생존 체온 (이보다 낮으면 체력 손실 등 발생)
    public float maxBodyTemperature = 42f; // 최고 생존 체온 (이보다 높으면 과열 상태)

    [Tooltip("환경 온도에 따라 체온이 얼마나 빠르게 변화하는지 (값이 클수록 빠르게 반응함)")]
    public float thermalAdjustmentSpeed = 0.5f;

    private void Awake()
    {
        // 싱글턴 인스턴스 설정 (중복 생성 방지)
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    private void Update()
    {
        if (dayNightCycle == null)
        {
            Debug.LogWarning("DayNightCycle이 연결되지 않았습니다.");
            return;
        }

        float time = dayNightCycle.time;

        // 현재 시간에 따른 온도 계산
        currentTemperature = temperatureCurve.Evaluate(time);

        // 디버그 로그로 현재 온도 출력
        Debug.Log($"[TemperatureManager] 현재 온도: {currentTemperature:F1}°C (시간: {time:F2})");

        // 매 프레임 플레이어 체온을 현재 월드 온도에 맞춰 서서히 변화시킴
        UpdatePlayerBodyTemperature();
    }

    /// <summary>
    /// 현재 월드 온도를 반환 (필요시 외부에서 읽을 수 있음)
    /// </summary>
    public float GetTemperature()
    {
        return currentTemperature;
    }

    /// <summary>
    /// 현재 월드 온도와의 차이에 따라 플레이어 체온을 조금씩 조절함
    /// </summary>
    /// 
    private void UpdatePlayerBodyTemperature()
    {
        float delta = currentTemperature - playerBodyTemperature;

        // 환경 온도와의 차이만큼 체온 조정
        playerBodyTemperature += delta * thermalAdjustmentSpeed * Time.deltaTime;

        // 비현실적인 체온을 방지 (예: 50도 이상, 20도 이하 등)
        playerBodyTemperature = Mathf.Clamp(playerBodyTemperature, minBodyTemperature, maxBodyTemperature);
    }

    /// <summary>
    /// 저체온증 여부 (기준: 체온 34도 이하)
    /// 체력 감소, 이동 속도 저하 등과 연동 가능
    /// </summary>
    public bool IsHypothermia()
    {
        return playerBodyTemperature <= 34f;
    }

    /// <summary>
    /// 고체온증(열사병 등) 여부 (기준: 체온 39도 이상)
    /// 체력 감소, 시야 흐림 등의 효과와 연동 가능
    /// </summary>
    public bool IsHyperthermia()
    {
        return playerBodyTemperature >= 39f;
    }
}
