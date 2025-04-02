using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingGameOver : MonoBehaviour
{
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            gameTimer.currentTime = 0f;
            Debug.Log("Game Over");
            Time.timeScale = 0f; // Pause the game
            // Game over logic here
        }
        Destroy(collision.gameObject);
    }
}
