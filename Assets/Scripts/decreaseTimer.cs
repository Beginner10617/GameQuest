using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class decreaseTimer : MonoBehaviour
{
    [SerializeField] private float timeToDecrease = 20f; // Time to decrease the timer by 5 seconds
    [SerializeField] private GameObject decreaseEffectPrefab; // Prefab for the effect
    [SerializeField] public RectTransform effectSpawnPoint; // Point where the effect will be spawned
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            SpawnDecreaseEffect();
            Destroy(gameObject);
        }   
    }
    public void DecreaseTimer()
    {
        gameTimer.currentTime -= timeToDecrease;
        Debug.Log("Decreased time by " + timeToDecrease + " seconds. Current time: " + gameTimer.currentTime);
    }
    void SpawnDecreaseEffect()
    {
        if (decreaseEffectPrefab != null && effectSpawnPoint != null)
        {
            GameObject effect = Instantiate(decreaseEffectPrefab, effectSpawnPoint);
            RectTransform rectTransform = effect.GetComponent<RectTransform>();
            effect.GetComponent<DecreaseEffect>().timeToDecrease = timeToDecrease; // Set the time to decrease
            rectTransform.anchoredPosition = Vector2.zero;  // Set to center
            rectTransform.localScale = Vector3.one;
            effect.GetComponent<DecreaseEffect>().text = "-" + timeToDecrease.ToString() + "s";
        }
    }
}
