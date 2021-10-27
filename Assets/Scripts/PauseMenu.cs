using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool isPaused = false;
    public GameObject pauseUI;

    //public Scene mainMenu;

    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Resume()
    {
        pauseUI.SetActive(false);
        Time.timeScale = 1.0f;
        isPaused = false;
    }

    public void LoadMenu()
    {
        Time.timeScale = 1.0f;
        Debug.Log("Return to main menu");
        SceneManager.LoadScene("MainMenu");
    }

    public void QuitGame()
    {
        Debug.Log("Quitting game");
        Application.Quit();
    }

    void Pause()
    {
        pauseUI.SetActive(true);
        Time.timeScale = 0.0f;
        isPaused = true;
    }
}
