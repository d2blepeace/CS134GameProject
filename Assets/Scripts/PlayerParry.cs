using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/**
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

    private bool parryActive;
    private bool onCooldown;
    void Awake()
    {
        // parryActive check is false if there is not any thing to trigger the parry
        if (parryTrigger != null) parryTrigger.enabled = false;

    }
    void OnParry(InputValue parryValue)
    {
        // ignore input if parry button isn't pressed and cooldown is still occuring
        if (!parryValue.isPressed || onCooldown) return;

        // Start Parry
        StartCoroutine(DoParry());
    }

    // Handle parry timing 
    private IEnumerator DoParry()
    {
        // Set everything active immediately
        onCooldown = true;
        parryActive = true;

        // Enable parry detection and wait for parry window
        if (parryTrigger != null) {parryTrigger.enabled = true;}
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
