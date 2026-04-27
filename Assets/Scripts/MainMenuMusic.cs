using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Controls the main menu's background music volume via a UI slider
// Reads the AudioSource's initial volume as the slider's starting value
public class MainMenuMusic : MonoBehaviour
{
    [Header("Audio Source")]
    [SerializeField] private AudioSource musicAudioSource;
    [Header("UI")]
    [SerializeField] private Slider musicVolumeSlider;
    // Start is called before the first frame update
    void Start()
    {
        if (musicVolumeSlider != null && musicAudioSource != null)
        {
            musicVolumeSlider.value = musicAudioSource.volume;
            musicVolumeSlider.onValueChanged.AddListener(SetMusicVolume);
        }
    }

    public void SetMusicVolume(float volume)
    {
        if (musicAudioSource != null)
            musicAudioSource.volume = volume;
    }
}
