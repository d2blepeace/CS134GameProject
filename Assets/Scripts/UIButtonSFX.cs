using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class UIButtonSFX : MonoBehaviour, IPointerEnterHandler
{
    [Header("SFX")]
    [SerializeField] private AudioSource sfxAudioSource;
    [SerializeField] private AudioClip hoverSFX;
    [SerializeField] private AudioClip clickSFX;

    private Button button;

    void Awake()
    {
        button = GetComponent<Button>();

        if (button != null)
            button.onClick.AddListener(PlayClickSFX);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (sfxAudioSource != null && hoverSFX != null)
            sfxAudioSource.PlayOneShot(hoverSFX);
    }

    private void PlayClickSFX()
    {
        if (sfxAudioSource != null && clickSFX != null)
            sfxAudioSource.PlayOneShot(clickSFX);
    }
}
