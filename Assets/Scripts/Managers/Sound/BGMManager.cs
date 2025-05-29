using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class BGMManager : MonoBehaviour
{
    AudioSource audioSource;
    public AudioClip[] horrorClips;

    float time;
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        time += Time.deltaTime;
        if(time > Random.Range(200f, 300f))
        {
            BGMPlay();
            time = 0f;
        }
    }

    public void BGMPlay()
    {
        audioSource.clip = horrorClips[Random.Range(0, horrorClips.Length)];
        audioSource.Play();
    }
}
