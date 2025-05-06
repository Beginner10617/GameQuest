using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    [SerializeField] private float timeToIncrease = 20f; // Time to decrease the timer by 5 seconds
    [SerializeField] private GameObject decreaseEffectPrefab; // Prefab for the effect
    [SerializeField] public RectTransform effectSpawnPoint; // Point where the effect will be spawned
    [SerializeField] private AudioClip collectSound; // Sound to play when the power-up is collected
    private AudioSource _audioSource; // Reference to the AudioSource component
    private void Start()
    {
        _audioSource = Camera.main.GetComponent<AudioSource>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Collision detected with: " + collision.gameObject.name);
        if(collision.gameObject.tag == "Player")
        {
            gameTimer.currentTime += timeToIncrease;
            Debug.Log("Increased time by " + timeToIncrease + " seconds. Current time: " + gameTimer.currentTime);
            SpawnIncreaseEffect();
            Debug.Log("Spawned increase effect");
            if (collectSound != null)
            {
                _audioSource.PlayOneShot(collectSound);
                Debug.Log("Played collect sound");
            }
            Destroy(gameObject);
        }
    }//

    void SpawnIncreaseEffect()
    {
        if (decreaseEffectPrefab != null && effectSpawnPoint != null)
        {
            GameObject effect = Instantiate(decreaseEffectPrefab, effectSpawnPoint);
            RectTransform rectTransform = effect.GetComponent<RectTransform>();
            effect.GetComponent<DecreaseEffect>().timeToDecrease = timeToIncrease; // Set the time to decrease
            rectTransform.anchoredPosition = Vector2.zero;  // Set to center
            rectTransform.localScale = Vector3.one;
            effect.GetComponent<DecreaseEffect>().text = "+" + timeToIncrease.ToString() + "s";
        }
    }
}