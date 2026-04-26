using UnityEngine;

public class LevelMusic : MonoBehaviour
{
    [Header("Music")]
    [SerializeField] private AudioSource musicAudioSource;

    void Start()
    {
        if (musicAudioSource != null) musicAudioSource.Play();
    }

    public void StopMusic()
    {
        if (musicAudioSource != null) musicAudioSource.Stop();
    }
}