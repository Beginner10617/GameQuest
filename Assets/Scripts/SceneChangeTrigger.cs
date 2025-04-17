using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneChangeTrigger : MonoBehaviour
{
    [SerializeField] private string sceneToLoad;
    float timeReset;
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // Load the specified scene
            ChangeScene();
        }
    }
    void Start()
    {
        Time.timeScale = 1f; // Ensure the game is running at normal speed
        timeReset = gameTimer.currentTime;
    }
    public void ChangeScene()
    {
        // Load the specified scene
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneToLoad);
    }
    public void Reset()
    {
        gameTimer.currentTime = timeReset;
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
    }
}
