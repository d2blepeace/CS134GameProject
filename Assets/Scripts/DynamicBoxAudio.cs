using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Plays an impact sound effect when the player collides with this object
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