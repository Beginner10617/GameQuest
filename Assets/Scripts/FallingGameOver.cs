using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingGameOver : MonoBehaviour
{
    void OisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            gameTimer.currentTime = 0f;
            Debug.Log("Game Over");
            // Game over logic here
        }
    }
}
