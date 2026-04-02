using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverUI : MonoBehaviour
{   
    [Header("UI Root")]
    [SerializeField] private GameObject gameOverPanel;

    // Start is called before the first frame update
    void Start()
    {
        gameOverPanel.SetActive(false);
    }

    public void ShowGameOver()
    {
        gameOverPanel.SetActive(true);
        //pause game
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    //Player click retry, reload the scence
    public void Retry()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    //Player click mainmenu, get back to main menu
    public void MainMenu()
    {
        Time.timeScale = 1f;
        // Implement Menu scene later
        Debug.Log("Main Menu not implemented yet.");

    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
