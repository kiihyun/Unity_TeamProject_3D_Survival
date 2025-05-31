using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public AudioSource musicsource;
    
    public AudioSource btnsource;

    public void SetMusicVolume(float volume)
    {
        if (musicsource != null)
        {
            musicsource.volume = volume;
        }
        else
        {
            Debug.LogWarning("Music source is not assigned in SoundManager.");
        }
    }

    public void OnSfx()
    {
        btnsource.Play();
    }
}
