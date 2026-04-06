using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HowToPlayUI : MonoBehaviour
{
    [Header("How to play UI References")]
    [SerializeField] private GameObject howToPlayPanel;
    [SerializeField] private PlayerHealth playerHealth;
    [SerializeField] private CameraController cameraController;

    private bool isToggle = false;

    void Start()
    {
        if (howToPlayPanel != null)
            howToPlayPanel.SetActive(false);

        if (playerHealth == null)
            playerHealth = FindObjectOfType<PlayerHealth>();
        if (cameraController == null && Camera.main != null)
            cameraController = Camera.main.GetComponent<CameraController>();
    }

    // Press H to toggle on/off how to play menu
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            if (playerHealth != null && playerHealth.isDead) return;
            ToggleHowToPlay();
        }
    }
    public void ToggleHowToPlay()
    {
        isToggle = !isToggle;

        if (howToPlayPanel != null)
            howToPlayPanel.SetActive(isToggle);

        if (isToggle)
        {
            Time.timeScale = 0f;

            if (cameraController != null)
                cameraController.enabled = false;

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            Time.timeScale = 1f;
            if (cameraController != null)
                cameraController.enabled = true;

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }
}
