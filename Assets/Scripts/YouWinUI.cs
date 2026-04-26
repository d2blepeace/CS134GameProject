using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class YouWinUI : MonoBehaviour
{   
    [Header("UI")]
    [SerializeField] private GameObject youWinPanel;
    [SerializeField] private GameObject nextLevelButton;
    [SerializeField] private CameraController cameraController;
    [SerializeField] private LevelMusic levelMusic;
    private bool hasWon = false;

    // Start is called before the first frame update
    void Start()
    {
        if (youWinPanel != null) youWinPanel.SetActive(false);

        if (cameraController == null && Camera.main != null)
            cameraController = Camera.main.GetComponent<CameraController>();

        if (levelMusic == null) levelMusic = FindObjectOfType<LevelMusic>();
    }

    public void ShowYouWin()
    {
        if (hasWon) return;
        hasWon = true;

        string currentScene = SceneManager.GetActiveScene().name;

        if (youWinPanel != null) youWinPanel.SetActive(true);

        if (nextLevelButton != null)
        {
            bool showNext = currentScene == "Level_1" || currentScene == "Level_2";
            nextLevelButton.SetActive(showNext);
        }

        SaveProgress();

        Time.timeScale = 0f;

        if (cameraController != null) cameraController.enabled = false;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        
        if (levelMusic != null)levelMusic.StopMusic();
    }

    public void SaveProgress()
    {
        string currentScene = SceneManager.GetActiveScene().name;

        if (currentScene == "Level_1")
        {
            PlayerPrefs.SetString("ResumeScene", "Level_2");
            PlayerPrefs.SetInt("HasResume", 1);
        }
        else if (currentScene == "Level_2")
        {
            PlayerPrefs.SetString("ResumeScene", "Level_3");
            PlayerPrefs.SetInt("HasResume", 1);
        }
        else if (currentScene == "Level_3")
        {
            PlayerPrefs.DeleteKey("ResumeScene");
            PlayerPrefs.SetInt("HasResume", 0);
        }

        PlayerPrefs.Save();
    }
    public void LoadNextLevel()
    {
        Time.timeScale = 1f;

        string currentScene = SceneManager.GetActiveScene().name;

        if (currentScene == "Level_1")
        {
            SceneManager.LoadScene("Level_2");
        }

        else if (currentScene == "Level_2")
        {
            SceneManager.LoadScene("Level_3");
        }

    }
    public void ReturnToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }
}
