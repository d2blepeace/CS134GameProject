using UnityEngine;

public class LevelMusic : MonoBehaviour
{
    [Header("Music")]
    [SerializeField] private AudioSource musicAudioSource;

    [Header("Pause Volume")]
    // Decrease volume to 30% when pause
    [SerializeField] private float pausedVolumeMultiplier = 0.3f;

    private float normalVolume = 1f;

    void Start()
    {
        if (musicAudioSource != null)
        {
            normalVolume = musicAudioSource.volume;
            musicAudioSource.Play();
        }
    }

    public void LowerMusicForPause()
    {
        if (musicAudioSource != null)
            musicAudioSource.volume = normalVolume * pausedVolumeMultiplier;
    }

    public void RestoreMusicAfterPause()
    {
        if (musicAudioSource != null)
            musicAudioSource.volume = normalVolume;
    }

    public void StopMusic()
    {
        if (musicAudioSource != null)
            musicAudioSource.Stop();
    }
}