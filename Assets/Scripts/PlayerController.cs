using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
/*
- Handles player movement, jumping, camera-relative input, collectible pickups, and win condition
- Requires a Rigidbody and PlayerHealth component on the same GameObject.
- Uses the New Input System callbacks 
*/
public class PlayerController : MonoBehaviour
{
    private CameraController cameraController;
    private Vector2 lookInput;
    // Pickup tracking
    private int count;
    private int totalPickups;
    // movements cached
    private float movementX;
    private float movementY;

    [Header("Movement")]
    [SerializeField] private Rigidbody rb;
    [SerializeField] private float speed = 0;
    [SerializeField] private float jumpForce = 8;
    [SerializeField] private float turnSpeed = 12f;
    [SerializeField] private Transform cameraTransform;
    
    [Header("UI")]
    [SerializeField ]public TextMeshProUGUI countText;
    [SerializeField] private YouWinUI youWinUI;    
    
    [Header("SFX")]
    [SerializeField] private AudioClip pickupSound;
    [SerializeField] private AudioSource pickupAudioSource;
    [SerializeField] private AudioClip jumpSound;
    [SerializeField] private AudioSource jumpAudioSource;

    // Health system
    private PlayerHealth playerHealth;
    //Prevent double jump
    private bool isGrounded;

    [Header("Ground Check - Prevent Double Jump")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckRadius = 0.2f;
    [SerializeField] private LayerMask groundLayer;

    // Start is called before the first frame update
    void Start()
    {
        // Only find rigidBody if focus on Player Object
        rb = GetComponent<Rigidbody>();

        //Point
        count = 0;

        // Cache camera references for camera-relative movement
        if (cameraTransform == null && Camera.main != null)
        {
            cameraTransform = Camera.main.transform;
        }
        cameraController = Camera.main.GetComponent<CameraController>();

        // Find playerHealth
        playerHealth = GetComponent<PlayerHealth>();
        
        // Count all collectibles in the scene for the win condition
        totalPickups = GameObject.FindGameObjectsWithTag("PickUp").Length;

        //Display initial point
        countText.text = "Point: 0 / " + totalPickups;

        // Activate win UI
        if (youWinUI == null) youWinUI = FindObjectOfType<YouWinUI>();

        //Count text
        SetCountText();
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
        //prevent double jump
        if (!jumpValue.isPressed) return;
        if (!isGrounded) return;

        if (jumpValue.isPressed)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);

            // Play jumpSFX
            if (jumpAudioSource != null && jumpSound != null)
            {
                jumpAudioSource.PlayOneShot(jumpSound);
            }
        }
    }
    void OnLook(InputValue lookValue)
    {
        lookInput = lookValue.Get<Vector2>();
        cameraController.SetLookInput(lookInput);
    }

    // Update is called once per frame
    void FixedUpdate()
    {     
        // Update ground check to prevent double jump
        if (groundCheck != null)
        {
            isGrounded = Physics.CheckSphere(groundCheck.position, groundCheckRadius, groundLayer);
        }
        if (cameraTransform == null) return;

        // Flatten camera axes onto the horizontal plane
        Vector3 cameraForward = cameraTransform.forward;
        Vector3 cameraRight = cameraTransform.right;
        cameraForward.y = 0f;
        cameraRight.y = 0f;
        cameraForward.Normalize();
        cameraRight.Normalize();

        // Combine input with camera-relative directions
        Vector3 moveDirection = cameraForward * movementY + cameraRight * movementX;
        if (moveDirection.sqrMagnitude > 1f) moveDirection.Normalize();

        rb.AddForce(moveDirection * speed, ForceMode.Force);

        // Smoothly rotate toward movement direction
        if (moveDirection.sqrMagnitude > 0.001f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            Quaternion smoothRotation = Quaternion.Slerp(rb.rotation, targetRotation, turnSpeed * Time.fixedDeltaTime);
            rb.MoveRotation(smoothRotation);
        }
    }
    
    // Trigger-based pickup collection. Pickups must be tagged "PickUp".
    void OnTriggerEnter(Collider other)
    {   
        if (other.gameObject.CompareTag("PickUp")) 
        {
            // Play pickup sound
            if (pickupAudioSource != null && pickupSound != null)
                pickupAudioSource.PlayOneShot(pickupSound, 1f);

            other.gameObject.SetActive(false);

            // Increase the count when collect pickup
            count++;            

            SetCountText();
        }   
    }

    // Contact damage from enemies on physical collision.
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            //Delegate to PlayerHealth to handle to wintext
            playerHealth.TakeDamage(1);
        }
    }

    // Updates the pickup counter UI and triggers the win screen
    void SetCountText()
    {
        int remainingPickups = GameObject.FindGameObjectsWithTag("PickUp").Length;

        if (countText != null)
        {
            countText.text = "Point: " + count + " / " + totalPickups;
        }

        // if there is no PickUp objects left, show youWinUI
        if (remainingPickups == 0)
        {
            if (youWinUI != null)
                youWinUI.ShowYouWin();
        }
    }
}
