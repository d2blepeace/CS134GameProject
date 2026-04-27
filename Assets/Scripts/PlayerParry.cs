using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/**
    Timed parry system 

    This is how parry system working:
    Player press Parry -> OnParry() called -> StartCoroutine() call DoParry() 
    -> Enable ParryZone collider -> Projectile enters trigger -> ParryZone checks IsParryActive 
    -> projectile got reflected back
*/

public class PlayerParry : MonoBehaviour
{
    [SerializeField] private float parryWindow = 0.2f;      //How long parry window stay active
    [SerializeField] private float parryCooldown = 0.25f;   //Time between parry to avoid spamming
    [SerializeField] private Collider parryTrigger;   //Sphere trigger around player

    [Header("Animation")]
    [SerializeField] private Animator animator;

    [Header("SFX")]
    [SerializeField] private AudioClip parryActiveSfx;
    [SerializeField] private AudioSource parryAudioSource;
    [SerializeField] private AudioClip parrySuccessSfx;


    private bool parryActive;
    private bool onCooldown;
    void Awake()
    {
        // Start with the parry trigger disabled; it only activates during the parry window
        if (parryTrigger != null) parryTrigger.enabled = false;

    }

    // initiates the parry if not on cooldown
    void OnParry(InputValue parryValue)
    {
        // ignore input if parry button isn't pressed and cooldown is still occuring
        if (!parryValue.isPressed || onCooldown) return;

        // Start Parry
        StartCoroutine(DoParry());
    }

    /// Called by Projectile.OnTriggerEnter when a successful deflection occur
    public void PlayParrySuccessSfx()
    {
        if (parryAudioSource != null && parrySuccessSfx != null)
        {
            parryAudioSource.PlayOneShot(parrySuccessSfx, 0.5f);
        }
    }

    /** Handle parry timing
        - 1. Enter cooldown immediately to block re-activation
        - 2. Play animation and SFX, enable the trigger collider
        -3. Wait for the parry window duration
        - 4. Deactivate the trigger, then wait out the remaining cooldown.
    */
    private IEnumerator DoParry()
    {
        // Set everything active immediately
        onCooldown = true;

        // play parry animation
        if (animator != null) animator.SetTrigger("Parry");

        // Play parrysfx
        if ( parryAudioSource != null && parryActiveSfx != null) parryAudioSource.PlayOneShot(parryActiveSfx);  

        parryActive = true;
        
        // Enable parry detection and wait for parry window
        if (parryTrigger != null) {parryTrigger.enabled = true;}

        // wait for parry window for a short time
        yield return new WaitForSeconds(parryWindow);
        // end parry window 
        parryActive = false;

        //disable trigger collider and wait for cooldown time before another parry
        if (parryTrigger != null) {parryTrigger.enabled = false;}
        yield return new WaitForSeconds(parryCooldown);

        // cooldown finished
        onCooldown = false;
    }
    public bool IsParryActive => parryActive;       //check if Player is currently parrying. 
}
