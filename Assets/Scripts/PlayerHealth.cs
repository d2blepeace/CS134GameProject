using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
/**
- Manages the player's health pool, damage intake, death sequence, and health UI
- Attach to the same GameObject as PlayerController and PlayerParry
- On death: disables player input, stops all enemies, shows Game Over UI
*/
public class PlayerHealth : MonoBehaviour
{
    [Header("Health Setting")]
    [SerializeField] private int maxHealth;
    [Header("Y to fall to death")]
    [SerializeField] private float fallDeathY = -50f;

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
    [SerializeField] private LevelMusic levelMusic;
    //Check if player is daed or not (read onlya ccess)
    public bool isDead
    { get; private set;} = false;

    void Start()
    {
        currHealth = maxHealth;
        UpdateHealthUI();
        if (levelMusic == null) levelMusic = FindObjectOfType<LevelMusic>();
    }
        void Update()
    {
        if (isDead) return;

        // Kill the player if they fall below the death threshold
        if (transform.position.y < fallDeathY)
        {
            Die();
        }
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
                hurtAudioSource.PlayOneShot(hurtSound);
            }
        }
    }

    // Die
    public void Die()
    {   
        if (isDead) return;
        isDead = true;

        // play deathsound and stop music
        if (deathSound != null && deathAudioSource != null) 
            deathAudioSource.PlayOneShot(deathSound);
        if (levelMusic != null) levelMusic.StopMusic();

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

        // Stop all enemies movement and attack
        EnemyAI[] enemies = FindObjectsOfType<EnemyAI>();
        foreach (EnemyAI enemy in enemies)
        {
            enemy.StopEnemy();
        }
        
        // Show gameOver UI
        if (gameOverUI != null) gameOverUI.ShowGameOver();
    }

    //Update UI of health based on currHealth (need refine this)
    private void UpdateHealthUI()
    {
        if (healthText!=null)
        {
            healthText.text = "Health: " + currHealth + " / " + maxHealth;
        }
    }
}
