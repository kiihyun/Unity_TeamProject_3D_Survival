using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalAudio : MonoBehaviour
{
    public AudioClip idleSound;
    public AudioClip deathSound;

    private AudioSource audioSource;
    private PassiveAnimal enemy; // 몹 상태 관리 스크립트

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        enemy = GetComponent<PassiveAnimal>();
        if (audioSource == null)
            Debug.LogError("AudioSource가 없습니다! 프리팹에 추가하세요.");
    }

    private void Update()
    {
        switch (enemy.aiState)
        {
            case AIState.Idle:
            case AIState.Attacking:
                PlaySoundOnce(idleSound); 
                break;
            case AIState.Death:
                PlayDeathSound();
                break;
        }
    }
    private void PlaySoundOnce(AudioClip clip)
    {
        if (!audioSource.isPlaying)
        {
            audioSource.volume = 0.05f;
            audioSource.maxDistance = 10f;
            audioSource.spatialBlend = 1f;
            audioSource.clip = clip;
            audioSource.Play();
        }
    }

    public void PlayDeathSound()
    {
        audioSource.volume = 0.05f;
        audioSource.maxDistance = 10f;
        audioSource.spatialBlend = 1f;
        audioSource.PlayOneShot(deathSound);
    }
}
