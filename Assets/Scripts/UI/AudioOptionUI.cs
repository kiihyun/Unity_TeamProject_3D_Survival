using UnityEngine;
using UnityEngine.UI;

public class AudioOptionUI : MonoBehaviour
{
    public Slider bgmSlider;
    public Slider sfxSlider;
    public SoundManager soundManager;

    public void OnBGMChanged(float value)
    {
        SettingsManager.Instance.bgmVolume = value;
        soundManager.SetMusicVolume(value);
    }

    public void OnSFXChanged(float value)
    {
        SettingsManager.Instance.sfxVolume = value;
        soundManager.SetSfxVolume(value);
    }
}
