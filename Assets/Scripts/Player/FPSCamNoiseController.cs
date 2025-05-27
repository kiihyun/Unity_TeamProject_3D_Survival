using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public enum eState
{
    Idle,
    Walk,
    Sprint
}

public class FPSCamNoiseController : MonoBehaviour
{
    //해당 오브젝트를 플레이어의 Input Action에 넣어주세요
    [Header("Gain Settings")]
    public eState curState;  //현재 상태

    [Header("Idle")]    //기본 상태의 흔들림 폭과 주기
    public float amplitudeOnIdle = 0.5f;
    public float frequencyOnIdle = 0.5f;

    [Header("Walk")]    //걷는 상태의 흔들림 폭과 주기
    public bool isWalk;
    public float amplitudeOnWalk = 0.2f;
    public float frequencyOnWalk = 0.02f;

    [Header("Run")]     //달리는 상태의 흔들림 폭과 주기
    public bool isRun;
    public float amplitudeOnRun = 0.3f;
    public float frequencyOnRun = 0.04f;

    [Header("Components")]
    public NoiseSettings idleSetting;
    public NoiseSettings walkSetting;
    public NoiseSettings runSetting;
    private CinemachineVirtualCamera FPS_cam;   //1인칭 시네머신 컴포넌트
    private CinemachineBasicMultiChannelPerlin noise;   //1인칭 시네머신의 노이즈

    private void Awake()
    {
        FPS_cam = GetComponent<CinemachineVirtualCamera>();
        noise = FPS_cam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }

    void Start()
    {
        curState = eState.Idle;  //상태 초기화
    }

    //걷기 입력
    public void OnMoveInput(InputAction.CallbackContext context)
    {
        if (!isRun)
        {
            if (context.phase == InputActionPhase.Performed && !isRun)
            {
                isWalk = true;
                StateSwitch(eState.Walk);
            }
            else if (context.phase == InputActionPhase.Canceled && !isRun)
            {
                isWalk = false;
                StateSwitch(eState.Idle);
            }
        }
    }

    //달리기 입력
    public void OnSprintInput(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            isRun = true;
            StateSwitch(eState.Sprint);
        }
        else if (context.phase == InputActionPhase.Canceled)
        {
            isRun = false;
            if (isWalk)
            {
                StateSwitch(eState.Walk);
            }
            else
            {
                StateSwitch(eState.Idle);
            }
        }
    }

    //상태 스위치 기능
    public void StateSwitch(eState _state)
    {
        curState = _state;
        switch (curState)
        {
            case eState.Idle:
                NoiseHandler(idleSetting, amplitudeOnIdle, frequencyOnIdle);
                break;
            case eState.Walk:
                NoiseHandler(walkSetting, amplitudeOnWalk, frequencyOnWalk);
                break;
            case eState.Sprint:
                NoiseHandler(runSetting, amplitudeOnRun, frequencyOnRun);
                break;
        }
    }

    //노이즈 값 설정 기능
    public void NoiseHandler(NoiseSettings _setting, float _amplitude, float _frequency)
    {
        noise.m_NoiseProfile = _setting;
        noise.m_AmplitudeGain = _amplitude;
        noise.m_FrequencyGain = _frequency;
    }
}
