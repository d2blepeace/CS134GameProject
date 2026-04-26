using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicBoxAudio : MonoBehaviour
{
    [Header("SFX")]
    [SerializeField] private AudioClip boxHitSound;
    [SerializeField] private AudioSource audioSource;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (audioSource != null && boxHitSound != null)
            {
                audioSource.PlayOneShot(boxHitSound, 2f);

            }
        }
    }
}