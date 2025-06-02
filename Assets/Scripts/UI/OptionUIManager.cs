using UnityEngine;
using UnityEngine.SceneManagement;

public class OptionUIManager : MonoBehaviour
{
    [Header("참조 컴포넌트")]
    public VideoOption videoOption;     // 해상도 관련 설정
    public Brightness brightness;       // 밝기 설정
    public SoundManager soundManager;   // 사운드 설정

    // 확인 버튼 클릭 시 호출할 메서드
    public void OnClickConfirm()
    {
        // 현재 UI의 설정을 SettingsManager에 저장
        SettingsManager.Instance.resolutionIndex = videoOption.resolutionNum;
        SettingsManager.Instance.isFullscreen = videoOption.fullscreenBtn.isOn;
        SettingsManager.Instance.brightness = brightness.birghtnessSlider.value;
        SettingsManager.Instance.bgmVolume = soundManager.musicsource.volume;
        SettingsManager.Instance.sfxVolume = soundManager.sfxVolume; // 따로 float 변수로 갖고 있어야 함

        // 설정 적용
        SettingsManager.Instance.ApplyAllSettings(videoOption.resolutions);
    }
}
