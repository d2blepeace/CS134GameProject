using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerHealth : MonoBehaviour
{
    [Header("Health Setting")]
    [SerializeField] private int maxHealth;

    [Header("UI")]
    [SerializeField] private TextMeshProUGUI healthText;
    [SerializeField] private GameObject winTextObject;
    [SerializeField] private GameOverUI gameOverUI;
    private int currHealth;
    [Header("SFX")]
    [SerializeField] private AudioClip hurtSound;
    [SerializeField] private AudioClip deathSound;
    [SerializeField] private AudioSource hurtAudioSource;
    [SerializeField] private AudioSource deathAudioSource;
    //Check if player is daed or not (read onlya ccess)
    public bool isDead
    { get; private set;} = false;

    void Start()
    {
        currHealth = maxHealth;
        UpdateHealthUI();
    }

    // if player take damage, player hurtsound, call UpdateHealth(), if health<=0 == die
    public void TakeDamage(int dmg)
    {
        // Ignore any further dmg if dead already
        if (isDead) return;
        Debug.Log($"TakeDamage called. dmg={dmg}, currHealth before={currHealth}");
        
        // take dmg, decreasecurrhealth
        currHealth -= dmg;
        currHealth = Mathf.Clamp(currHealth, 0, maxHealth);
        Debug.Log($"currHealth after={currHealth}");


        //Update ui based on health
        UpdateHealthUI();

        // if health <= 0, die
        if (currHealth <= 0) Die();
        else
        {
            // Play hurtsound
            if (hurtSound != null && hurtAudioSource != null)
            {
                hurtAudioSource.PlayOneShot(hurtSound, 1f);
            }
        }
    }

    // Die
    public void Die()
    {   
        if (isDead) return;
        isDead = true;

        // play deathsound at current camera position (player's position)
        if (deathSound != null && deathAudioSource != null)
        {
            deathAudioSource.PlayOneShot(deathSound, 1f);
        }

        // disable movement and parry 
        PlayerController controller = GetComponent<PlayerController>();
        if (controller != null) controller.enabled = false;
        PlayerParry parry = GetComponent<PlayerParry>();

        if (parry != null) parry.enabled = false;
        Rigidbody rb = GetComponent<Rigidbody>();

        //disable camera movement
        CameraController cam = Camera.main != null ? Camera.main.GetComponent<CameraController>() : null;
        cam.enabled = false;

        if (rb != null) rb.velocity = Vector3.zero;       

        // Show gameOver UI
        if (gameOverUI != null)
            gameOverUI.ShowGameOver();
    }

    //Update UI of health based on currHealth (need refine this)
    private void UpdateHealthUI()
    {
        if (healthText!=null)
        {
            healthText.text = "HP: " + currHealth + " / " + maxHealth;
        }
    }
}
