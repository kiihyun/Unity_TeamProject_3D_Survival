using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RiverSound : MonoBehaviour
{
    public AudioClip waterSound; //오디오 클립을 연결할 변수
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponentInParent<AudioSource>(); //오디오 소스 컴포넌트를 가져옴
        if (audioSource == null)
        {
            Debug.Log("AudioSource is null");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (audioSource.isPlaying)
            {
                audioSource.Pause(); //이미 재생 중이라면 일시 정지
            }
            else
            {
                audioSource.Play(); //재생 중이 아니라면 재생
            }
        }
        
    }
}
