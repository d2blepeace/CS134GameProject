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
        Vector3 cameraOffset = rotation * new Vector3(0f, 0f, -distance);

        transform.position = focusPoint + cameraOffset;
        transform.LookAt(focusPoint);
    }

    public void SetLookInput(Vector2 input)
    {
        lookInput = input;
    }
}
