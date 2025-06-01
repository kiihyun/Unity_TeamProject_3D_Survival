using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public AudioSource musicsource;
    public AudioSource btnsource;

    // 현재 설정된 SFX 볼륨 저장
    public float sfxVolume = 1f;

    public void SetMusicVolume(float volume)
    {
        if (musicsource != null)
        {
            musicsource.volume = volume;
        }
    }

    public void SetSfxVolume(float volume)
    {
        if (btnsource != null)
        {
            btnsource.volume = volume;
        }

        // 현재 볼륨을 저장
        sfxVolume = volume;
    }

    public void OnSfx()
    {
        btnsource.Play();
    }
}





//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class SoundManager : MonoBehaviour
//{
//    public AudioSource musicsource;
//    public AudioSource btnsource;

//    public void SetMusicVolume(float volume)
//    {
//        if (musicsource != null)
//        {
//            musicsource.volume = volume;
//        }
//        else
//        {
//            Debug.LogWarning("Music source is not assigned in SoundManager.");
//        }
//    }

//    public void OnBGMVolumeChanged(float value)
//    {
//        SettingsManager.Instance.bgmVolume = value;
//        SetMusicVolume(value);
//    }

//    public void OnSfxVolumeChanged(float value)
//    {
//        if (btnsource != null)
//        {
//            btnsource.volume = value;
//        }
//        else
//        {
//            Debug.LogWarning("Button sound source is not assigned in SoundManager.");

//        }
//    }

//    public void OnSfx()
//    {
//        btnsource.Play();
//    }
//}
