using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerHealth : MonoBehaviour
{
    [Header("Health Setting")]
    [SerializeField] private int maxHealth = 3;

    [Header("UI")]
    [SerializeField] private TextMeshProUGUI healthText;
    [SerializeField] private GameObject winTextObject;
    [SerializeField] private GameOverUI gameOverUI;

    [Header("Audio")]
    [SerializeField] private AudioClip hurtSound;
    [SerializeField] private AudioClip deathSound;

    private int currHealth;
    private AudioSource audioSource;
    
    //Check if player is daed or not (read onlya ccess)
    public bool isDead
    { get; private set;} = false;

    void Start()
    {
        currHealth = maxHealth;
        audioSource = GetComponent<AudioSource>();
        UpdateHealthUI();
    }

    // if player take damage, player hurtsound, call UpdateHealth(), if health<=0 == die
    public void TakeDamage(int dmg)
    {
        // Ignore any further dmg if dead already
        if (isDead) return;

        // take dmg, decreasecurrhealth
        currHealth -= dmg;
        currHealth = Mathf.Clamp(currHealth, 0, maxHealth);

        //Update ui based on health
        UpdateHealthUI();

        // if health <= 0, die
        if (currHealth <= 0) Die();
        else
        {
            // Play hurtsound
            if (hurtSound != null && audioSource != null)
            {
                audioSource.PlayOneShot(hurtSound, 1f);
            }
        }
    }

    // Die
    public void Die()
    {   
        if (isDead) return;
        isDead = true;

        // play deathsound at current camera position (player's position)
        if (deathSound != null && audioSource != null)
        {
            AudioSource.PlayClipAtPoint(deathSound, Camera.main.transform.position, 1f);
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
