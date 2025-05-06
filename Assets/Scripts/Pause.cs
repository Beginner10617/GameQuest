using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pause : MonoBehaviour
{
    public string mainMenuSceneName = "MainMenu";
    public GameObject pauseMenuUI;
    public void MainMenu()
    {
        Time.timeScale = 1;
        Debug.Log("Game Resumed");
        UnityEngine.SceneManagement.SceneManager.LoadScene(mainMenuSceneName);
    }
    public void PauseGame()
    {
        Time.timeScale = 0;
        Debug.Log("Game Paused");  
        pauseMenuUI.SetActive(true);      
    }
    public void ResumeGame()
    {
        Time.timeScale = 1;
        Debug.Log("Game Resumed");
        pauseMenuUI.SetActive(false);
    }
    void Start()
    {
        if(pauseMenuUI == null)
        {
            Debug.LogError("Pause Menu UI is not assigned in the inspector.");
            return;
        }
        pauseMenuUI.SetActive(false);
    }
    void Update()
    {
        if (InputManager.pausePressed)
        {
            if (Time.timeScale == 1)
            {
                PauseGame();
            }
            else
            {
                ResumeGame();
            }
        }
    }
}
