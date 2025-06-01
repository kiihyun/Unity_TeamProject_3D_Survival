using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAudio : MonoBehaviour
{
    public AudioClip idleSound;
    public AudioClip attackSound;
    public AudioClip deathSound;

    private AudioSource audioSource;
    private Enemy enemy; // 좀비 상태 관리 스크립트

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        enemy = GetComponent<Enemy>();
        if (audioSource == null)
            Debug.LogError("AudioSource가 없습니다! 프리팹에 추가하세요.");
    }

    private void Update()
    {
        switch (enemy.aiState)
        {
            case AIState.Idle:
            case AIState.Wandering:
                PlaySoundOnce(idleSound);
                break;
            case AIState.Attacking:
                PlaySoundOnce(attackSound);
                break;
            case AIState.Death:
                break;
        }
    }

    private void PlaySoundOnce(AudioClip clip)
    {
        if (!audioSource.isPlaying)
        {
            audioSource.clip = clip;
            Debug.Log("Current clip: " + audioSource.clip);
            Debug.Log("Volume: " + audioSource.volume);
            Debug.Log("Mute: " + audioSource.mute);
            audioSource.volume = 0.5f;
            audioSource.Play();
        }
    }

    public void PlayDeathSound()
    {
        audioSource.volume = 0.5f;
        audioSource.PlayOneShot(deathSound);
    }
}