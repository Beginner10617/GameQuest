using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneChangeTrigger : MonoBehaviour
{
    [SerializeField] private string sceneToLoad;
    float timeReset;
    [SerializeField] private bool isDoor = false;
    [SerializeField] private AudioClip doorSound;
    [SerializeField] private GameObject doorOpenPrompt;
    [SerializeField] private GameObject DoorObject;
    private AudioSource audioSource;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if(isDoor) doorOpenPrompt.SetActive(true);
            if (InputManager.openDoorPressed)
            {
                if(DoorObject != null) DoorObject.SetActive(false);
                ChangeScene();
                
            }
        }
    }
    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if(isDoor) doorOpenPrompt.SetActive(false);
        
            if (InputManager.openDoorPressed)
            {
                if (DoorObject != null) DoorObject.SetActive(false);
                ChangeScene();
            }
        }
    }
    void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (InputManager.openDoorPressed)
            {
                if (DoorObject != null) DoorObject.SetActive(false);
                ChangeScene();
            }
        }
    }
    void Start()
    {
        Time.timeScale = 1f; // Ensure the game is running at normal speed
        timeReset = gameTimer.currentTime;
        audioSource = GetComponent<AudioSource>();
        if(isDoor) doorOpenPrompt.SetActive(false); // Hide the prompt at the start
    }
    private IEnumerator WaitForSoundAndChangeScene()
    {
        // Wait for the sound to finish playing
        if (audioSource != null && doorSound != null)
        {
            yield return new WaitForSeconds(doorSound.length);
        }
        // Load the specified scene
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneToLoad);
    }
    public void ChangeScene()
    {
        // Check if the player is entering a door
        if (isDoor)
        {
            // Set the player's position to the door's position
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            player.transform.position = transform.position;
            // Play the door sound
            if (audioSource != null && doorSound != null && !audioSource.isPlaying)
            {
                audioSource.PlayOneShot(doorSound);
            }
            transform.eulerAngles = new Vector3(0, 0, 45);
            Invoke("GoToNextScene", doorSound.length); // Wait for the sound to finish before changing the scene
        }
        else
        {
            // Load the specified scene immediately
            UnityEngine.SceneManagement.SceneManager.LoadScene(sceneToLoad);
        }
    }
    void GoToNextScene()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneToLoad);
    }
    public void Reset()
    {
        gameTimer.currentTime = timeReset;
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
    }
}
