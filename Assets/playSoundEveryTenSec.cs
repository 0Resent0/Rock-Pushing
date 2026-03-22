using UnityEngine;

public class PlaySoundLoop : MonoBehaviour
{
    public AudioSource audioSource;
    public float interval = 10f;

    void Start()
    {
        InvokeRepeating(nameof(PlaySound), 0f, interval);
    }

    void PlaySound()
    {
        if (audioSource != null)
        {
            audioSource.Play();
        }
    }
}