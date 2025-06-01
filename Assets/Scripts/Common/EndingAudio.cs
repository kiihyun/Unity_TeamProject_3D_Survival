using UnityEngine;

public class EndingAudio : MonoBehaviour
{
    public AudioSource audioSource;

    void Start()
    {
        Invoke("PlaySound", 3f); // 3초 후 재생
    }

    void PlaySound()
    {
        audioSource.Play();
    }
}
