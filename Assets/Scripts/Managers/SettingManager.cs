using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SettingsManager : MonoBehaviour
{
    public static SettingsManager Instance;

    public int resolutionIndex = 0;
    public bool isFullscreen = true;
    public float brightness = 1f;
    public float bgmVolume = 1f;
    public float sfxVolume = 1f;

    private string previousSceneName; // 이전 씬 저장용

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // 씬 전환 시 유지
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SetPreviousScene(string sceneName)
    {
        previousSceneName = sceneName; // 외부에서 설정
    }

    public void ApplyAllSettings(List<Resolution> resolutions)
    {
        // 해상도
        var selected = resolutions[resolutionIndex];
        var mode = isFullscreen ? FullScreenMode.FullScreenWindow : FullScreenMode.Windowed;
        Screen.SetResolution(selected.width, selected.height, mode);

        // 밝기
        FindObjectOfType<Brightness>()?.AdjustBrightness(brightness);

        // 사운드
        var sound = FindObjectOfType<SoundManager>();
        if (sound != null)
        {
            sound.SetMusicVolume(bgmVolume);
            sound.SetSfxVolume(sfxVolume);
        }
    }

    public void OnConfirmBtn(List<Resolution> resolutions)
    {
        ApplyAllSettings(resolutions);

        // 이전 씬으로 돌아가기
        if (!string.IsNullOrEmpty(previousSceneName))
        {
            SceneManager.LoadScene(previousSceneName);
        }
        else
        {
            Debug.LogWarning("이전 씬 이름이 설정되지 않았습니다.");
        }
    }
}
