using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChange : MonoBehaviour
{
    
    public void ChangeScene(string name)
    {
        SceneManager.LoadScene(name);
    }
    public void ResetScene()
    {
        Time.timeScale = 1f; // Ensure the game is running at normal speed
        SceneManager.LoadScene(SceneManager.GetActiveScene().name.ToString());
    }
    public void ExitGame()
    {
        gameTimer.currentTime = 150;
        Application.Quit();
    }
}
