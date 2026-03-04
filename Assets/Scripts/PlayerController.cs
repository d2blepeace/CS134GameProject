using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;


public class PlayerController : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;
    private int count;
    private float movementX;
    private float movementY;
    
    [SerializeField] private float speed = 0;
    [SerializeField] private float jumpForce = 0;
    public TextMeshProUGUI countText;
    public GameObject winTextObject;
    [SerializeField] private AudioClip pickupSound;
    [SerializeField] private AudioClip deathSound;
    private AudioSource sfxSource;
    private AudioSource myAudioSource;

    // Start is called before the first frame update
    void Start()
    {
        // Only find rigidBody if focus on Player Object
        rb = GetComponent<Rigidbody>();
        count = 0;

        SetCountText();

        // Activate win text
        winTextObject.SetActive(false);

        //Get Audio source
        myAudioSource = GetComponent<AudioSource>();
        if (myAudioSource == null) myAudioSource = gameObject.AddComponent<AudioSource>();
    }

    // Called when input movement is found
    void OnMove(InputValue movementValue)
    {
        //Convert input value to Vector2 movement
        Vector2 movementVector = movementValue.Get<Vector2>(); 
        movementX = movementVector.x;
        movementY = movementVector.y;
    }

    // Jump when space is pressed
    void OnJump(InputValue jumpValue)
    {
        if (jumpValue.isPressed)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }
    void SetCountText()
    {
        countText.text = "Point: " + count.ToString();

        if (count >= 12)
        {
            winTextObject.SetActive(true);

            // Destroy enemy
            Destroy(GameObject.FindGameObjectWithTag("Enemy"));
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // Create 3D movement vector using X and Y inputs
        Vector3 movement = new Vector3(movementX, 0.0f, movementY);
        
        //Apply force to Rigidbody to move player
        rb.AddForce(movement * speed);
    }
    
    void OnTriggerEnter(Collider other)
    {   
        if (other.gameObject.CompareTag("PickUp")) 
        {
            // Play pickup sound
            myAudioSource.PlayOneShot(pickupSound, 0.7f);
            other.gameObject.SetActive(false);

            // Increase the count when collect pickup
            count++;            

            SetCountText();
        }   
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            // Destroy the current object and play death sound
            AudioSource.PlayClipAtPoint(deathSound, Camera.main.transform.position, 1f);
            Destroy(gameObject); 
            
            // Update the winText to display "You Lose!"
            winTextObject.gameObject.SetActive(true);
            winTextObject.GetComponent<TextMeshProUGUI>().text = "You Lose!";
        }

    }
}
