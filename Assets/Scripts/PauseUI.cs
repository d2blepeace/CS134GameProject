using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class PauseUI : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private PlayerHealth playerHealth;
    [SerializeField] private CameraController cameraController;
    [SerializeField] private HowToPlayUI howToPlayUI;

    private bool isPaused = false;
    public bool IsPaused => isPaused;

    void Start()
    {
        if (pausePanel != null)
            pausePanel.SetActive(false);

        if (playerHealth == null)
            playerHealth = FindObjectOfType<PlayerHealth>();

        if (cameraController == null && Camera.main != null)
            cameraController = Camera.main.GetComponent<CameraController>();

        if (howToPlayUI == null)
            howToPlayUI = FindObjectOfType<HowToPlayUI>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (playerHealth != null && playerHealth.isDead) return;
            if (howToPlayUI != null && howToPlayUI.IsOpen) return;

            TogglePause();
        }
    }

    public void TogglePause()
    {
        isPaused = !isPaused;

        if (pausePanel != null)
            pausePanel.SetActive(isPaused);

        if (isPaused)
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

    public void ResumeGame()
    {
        if (!isPaused) return;
        TogglePause();
    }

    public void OpenSettings()
    {
        //TODO Later:
        Debug.Log("Settings menu not implemented yet.");
    }

    public void ReturnToMainMenu()
    {
        Time.timeScale = 1f;
        // TODO Later:
        SceneManager.LoadScene("MainMenu");
    }

    public void ExitGame()
    {
        Time.timeScale = 1f;
        Debug.Log("Quit Game");

        Application.Quit();
    }
}