using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuUI : MonoBehaviour
{
    [Header("Scene Name")]
    [SerializeField] private string firstLevelSceneName = "Level_1";

    [Header("UI")]
    [SerializeField] private GameObject resumeButton;

    private void Start()
    {
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        // Fresh start of game, HasResume = 0, only played once hasResume = 1
        bool hasResume = PlayerPrefs.GetInt("HasResume", 0) == 1;

        if (resumeButton != null)
            resumeButton.SetActive(hasResume);
    }

    public void StartGame()
    {
        Time.timeScale = 1f;

        // Start from beginning
        PlayerPrefs.SetString("ResumeScene", firstLevelSceneName);
        PlayerPrefs.SetInt("HasResume", 1);
        PlayerPrefs.Save();

        SceneManager.LoadScene(firstLevelSceneName);
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f;

        string resumeScene = PlayerPrefs.GetString("ResumeScene", firstLevelSceneName);
        SceneManager.LoadScene(resumeScene);
    }

    public void OpenSetting()
    {
        Debug.Log("Settings not implemented yet.");
    }

    public void QuitGame()
    {
        Debug.Log("Quitting game...");
        Application.Quit();
    }
}