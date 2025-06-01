using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalAudio : MonoBehaviour
{
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
                break;
            case AIState.Death:
                PlayDeathSound();
                break;
        }
    }


    public void PlayDeathSound()
    {
        audioSource.PlayOneShot(deathSound);
    }
}
