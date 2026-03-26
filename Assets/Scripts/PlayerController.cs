using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;


public class PlayerController : MonoBehaviour
{
    private CameraController cameraController;
    private Vector2 lookInput;
    private int count;
    private float movementX;
    private float movementY;

    [Header("Movement")]
    [SerializeField] private Rigidbody rb;
    
    [SerializeField] private float speed = 0;
    [SerializeField] private float jumpForce = 0;
    [SerializeField] private float turnSpeed = 12f;
    [SerializeField] private Transform cameraTransform;
    
    [Header("UI")]
    public TextMeshProUGUI countText;
    public GameObject winTextObject;
    [Header("Sound")]
    [SerializeField] private AudioClip pickupSound;
    [SerializeField] private AudioClip deathSound;
    private AudioSource sfxSource;
    private AudioSource myAudioSource;

    // Health system
    private PlayerHealth playerHealth;

    // Start is called before the first frame update
    void Start()
    {
        // Only find rigidBody if focus on Player Object
        rb = GetComponent<Rigidbody>();
        count = 0;

        // Camera control
        if (cameraTransform == null && Camera.main != null)
        {
            cameraTransform = Camera.main.transform;
        }
        cameraController = Camera.main.GetComponent<CameraController>();

        // Find playerHealth
        playerHealth = GetComponent<PlayerHealth>();
    
        //Count text
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
    void OnLook(InputValue lookValue)
    {
        lookInput = lookValue.Get<Vector2>();
        cameraController.SetLookInput(lookInput);
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
        if (cameraTransform == null) return;

        Vector3 cameraForward = cameraTransform.forward;
        Vector3 cameraRight = cameraTransform.right;

        cameraForward.y = 0f;
        cameraRight.y = 0f;
        cameraForward.Normalize();
        cameraRight.Normalize();

        Vector3 moveDirection = cameraForward * movementY + cameraRight * movementX;

        if (moveDirection.sqrMagnitude > 1f)
        {
            moveDirection.Normalize();
        }

        rb.AddForce(moveDirection * speed, ForceMode.Force);

        if (moveDirection.sqrMagnitude > 0.001f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            Quaternion smoothRotation = Quaternion.Slerp(rb.rotation, targetRotation, turnSpeed * Time.fixedDeltaTime);
            rb.MoveRotation(smoothRotation);
        }
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
            //Delegate to PlayerHealth to handle to wintext
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(1);
            }
        }

    }
}
