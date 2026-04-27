using UnityEngine;

// Simple wrapper that starts level background music on scene load
//  and exposes a StopMusic() method for death / win / pause events
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