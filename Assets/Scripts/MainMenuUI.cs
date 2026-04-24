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

        bool hasResume = PlayerPrefs.GetInt("HasResume", 0) == 1;
        string resumeScene = PlayerPrefs.GetString("ResumeScene", "");

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
    public void SaveResumeScene(string sceneName)
    {
        PlayerPrefs.SetString("ResumeScene", sceneName);
        PlayerPrefs.SetInt("HasResume", 1);
        PlayerPrefs.Save();
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