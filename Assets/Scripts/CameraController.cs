using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    public Transform player;

    [Header("Orbit Settings")]
    [SerializeField] private float distance = 4f;
    [SerializeField] private float heightOffset = 1.2f;

    //Mouse sensitivity: 100% = 1f
    [SerializeField] private float mouseSensitivityX = 0.5f;
    [SerializeField] private float mouseSensitivityY = 0.5f;

    // How far can camera can look up and down
    [SerializeField] private float minPitch = -20f;             
    [SerializeField] private float maxPitch = 40f;
    private float yaw;
    private float pitch = 15f;
    private Vector2 lookInput;
    private Vector3 currentCameraPosition;

    //Camera colliosion
    [Header("Camera Collision")]
    [SerializeField] private LayerMask collisionMask;
    [SerializeField] private float cameraRadius = 0.25f;
    [SerializeField] private float minDistance = 1.0f;
    [SerializeField] private float collisionBuffer = 0.1f;
    [SerializeField] private float positionSmoothSpeed = 12f;

    // Start is called before the first frame update
    void Start()
    {
        if (player == null)
        {
            GameObject p = GameObject.FindGameObjectWithTag("Player");
            if (p!=null) player = p.transform;
        }

        Vector3 currRotation = transform.eulerAngles;
        yaw = currRotation.y;
        pitch = currRotation.x;

        currentCameraPosition = transform.position;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void OnLook(InputValue val)
    {
        lookInput = val.Get<Vector2>();
    }
    // Set camera position in LateUpdate(), run every frame after Update();
    void LateUpdate()
    {
        if (player == null) return;

        //Minimum and Maximum that camera can look up or down
        yaw += lookInput.x * mouseSensitivityX;
        pitch -= lookInput.y * mouseSensitivityY;
        pitch = Mathf.Clamp(pitch, minPitch, maxPitch);

        //Rotation relative to movement
        Quaternion rotation = Quaternion.Euler(pitch, yaw, 0f);
        Vector3 focusPoint = player.position + Vector3.up * heightOffset;
        Vector3 desiredOffset = rotation * new Vector3(0f, 0f, -distance);
        Vector3 desiredPosition = focusPoint + desiredOffset;
        Vector3 castDirection = desiredOffset.normalized;

        float targetDistance = distance;
        if (Physics.SphereCast(focusPoint, cameraRadius, castDirection, 
                                    out RaycastHit hit, distance, collisionMask, QueryTriggerInteraction.Ignore))
        {
            targetDistance = Mathf.Clamp(hit.distance - collisionBuffer, minDistance, distance);
        }

        Vector3 finalPosition = focusPoint + castDirection * targetDistance;
        currentCameraPosition = finalPosition;

        transform.position = currentCameraPosition;
        transform.LookAt(focusPoint);
    }

    public void SetLookInput(Vector2 input)
    {
        lookInput = input;
    }

    // For setting of mouse sensitivity
    public void SetMouseSensitivity(float sliderValue)
    {
        float convertedValue = Mathf.Clamp(sliderValue / 100f, 0.1f, 2f);
        mouseSensitivityX = convertedValue;
        mouseSensitivityY = convertedValue;
    }
    public void SetMouseSensitivityX(float value)
    {
        mouseSensitivityX = value;
    }

    public void SetMouseSensitivityY(float value)
    {
        mouseSensitivityY = value;
    }
}
