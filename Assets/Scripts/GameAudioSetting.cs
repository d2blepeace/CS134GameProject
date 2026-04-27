using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

// In-game audio settings panel controlling the level's background music
// Provides both a volume slider and a mute toggle
// When the toggle is off, volume is set to 0 regardless of the slider position
public class GameAudioSetting : MonoBehaviour
{
    [Header("Audio Sources")]
    [SerializeField] private AudioSource levelMusicSource;
    [Header("Toggles")]
    [SerializeField] private Toggle musicToggle;
    [Header("Sliders")]
    [SerializeField] private Slider musicSlider;
    private float musicVolume = 1f;

    void Start()
    {
        if (musicSlider != null)
        {
            musicSlider.value = musicVolume;
            musicSlider.onValueChanged.AddListener(SetMusicVolume);
        }

        if (musicToggle != null)
            musicToggle.onValueChanged.AddListener(SetMusicEnabled);

        ApplyMusicVolume();
    }

    public void SetMusicVolume(float value)
    {
        musicVolume = value;
        ApplyMusicVolume();
    }

    public void SetMusicEnabled(bool isOn)
    {
        ApplyMusicVolume();
    }
    
    // Sets the AudioSource volume to the slider value when the toggle is on,
    //or zero when muted.
    private void ApplyMusicVolume()
    {
        if (levelMusicSource == null || musicToggle == null) return;

        levelMusicSource.volume = musicToggle.isOn ? musicVolume : 0f;
    }
}