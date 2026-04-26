using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

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

    private void ApplyMusicVolume()
    {
        if (levelMusicSource == null || musicToggle == null) return;

        levelMusicSource.volume = musicToggle.isOn ? musicVolume : 0f;
    }
}