using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuUI : MonoBehaviour
{
    [Header("Scene Name")]
    [SerializeField] private string gameSceneName = "Level_1"; // change to playing scene name 

    // Start is called before the first frame update
    public void StartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(gameSceneName);
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
