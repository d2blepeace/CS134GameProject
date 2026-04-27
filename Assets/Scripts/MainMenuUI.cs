using UnityEngine;
using UnityEngine.SceneManagement;

/**
Main menu controller handling New Game, Resume, Settings, Tutorial, and Quit
    - Resume functionality uses PlayerPrefs ("HasResume" + "ResumeScene") to track
        the furthest level unlocked. The Resume button is hidden if no save exists
*/
public class MainMenuUI : MonoBehaviour
{
    [Header("Scene Name")]
    [SerializeField] private string firstLevelSceneName = "Level_1";

    [Header("UI")]
    [SerializeField] private GameObject resumeButton;
    [SerializeField] private GameObject mainMenuPanel;
    [SerializeField] private GameObject tutorialPanel;
    [SerializeField] private GameObject settingsPanel;

    private void Start()
    {   
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        bool hasResume = PlayerPrefs.GetInt("HasResume", 0) == 1;
        string resumeScene = PlayerPrefs.GetString("ResumeScene", "");

        // Show the Resume button only if saved progress exists
        if (resumeButton != null)
            resumeButton.SetActive(hasResume && !string.IsNullOrEmpty(resumeScene));
    }

    public void StartGame()
    {

        Time.timeScale = 1f;

        // New game should NOT create resume progress
        PlayerPrefs.DeleteKey("HasResume");
        PlayerPrefs.DeleteKey("ResumeScene");
        PlayerPrefs.Save();

        SceneManager.LoadScene(firstLevelSceneName);
    }
    
    // Wipe saved progress without starting a new game
    public void ClearSave()
    {
        PlayerPrefs.DeleteKey("HasResume");
        PlayerPrefs.DeleteKey("ResumeScene");
        PlayerPrefs.Save();
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f;

        string resumeScene = PlayerPrefs.GetString("ResumeScene", "");

        if (!string.IsNullOrEmpty(resumeScene))
        {
            SceneManager.LoadScene(resumeScene);
        }
        else
        {
            SceneManager.LoadScene(firstLevelSceneName);
        }
    }
    // Saves a scene name as the resume point (called by YouWinUI.SaveProgress)
    public void SaveResumeScene(string sceneName)
    {
        PlayerPrefs.SetString("ResumeScene", sceneName);
        PlayerPrefs.SetInt("HasResume", 1);
        PlayerPrefs.Save();
    }

    public void OpenSetting()
    {
        if (mainMenuPanel != null) mainMenuPanel.SetActive(false);
        if (settingsPanel != null) settingsPanel.SetActive(true);
    }

    public void CloseSetting()
    {
        if (settingsPanel != null) settingsPanel.SetActive(false); 
        if (mainMenuPanel != null) mainMenuPanel.SetActive(true);
    }
    public void OpenTutorial()
    {
        if (mainMenuPanel != null) mainMenuPanel.SetActive(false);
        if (tutorialPanel != null) tutorialPanel.SetActive(true);
    }
    public void CloseTutorial()
    {
        if (tutorialPanel != null) tutorialPanel.SetActive(false);
        if (mainMenuPanel != null) mainMenuPanel.SetActive(true);
    }
    public void QuitGame()
    {
        Debug.Log("Quitting game...");
        Application.Quit();
    }

}