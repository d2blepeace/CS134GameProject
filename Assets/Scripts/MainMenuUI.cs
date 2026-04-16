using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuUI : MonoBehaviour
{
    [Header("Scene Name")]
    [SerializeField] private string gameSceneName = "Level_1"; // change to playing scene name 
    [Header("For Resume Button")]
    [SerializeField] private GameObject resumeButton;

    void Start()
    {
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        bool hasResume = PlayerPrefs.GetInt("HasResume", 0) == 1;

        if (resumeButton != null)
            resumeButton.SetActive(hasResume);
    }
    // Start is called before the first frame update
    public void StartGame()
    {
        Time.timeScale = 1f;

        // Start new game from Level_1
        PlayerPrefs.SetString("ResumeScene", gameSceneName);
        PlayerPrefs.SetInt("HasResume", 1);
        PlayerPrefs.Save();

        SceneManager.LoadScene(gameSceneName);
    }

    //Resume game
    public void ResumeGame()
    {
        Time.timeScale = 1f;

        string resumeScene = PlayerPrefs.GetString("ResumeScene", gameSceneName);
        SceneManager.LoadScene(resumeScene);
    }
    // Settings button
    public void OpenSetting()
    {
        
    }

    // Quit game button
    public void QuitGame()
    {
        // for debugging in unity
        Debug.Log("Quitting game...");
        Application.Quit();
    }
}
