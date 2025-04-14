using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingGameOver : MonoBehaviour
{
    void Start()
    {
        gameOverUI.SetActive(false);
    }
    [SerializeField] private GameObject gameOverUI;
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
//            gameTimer.currentTime = 0f;
            Debug.Log("Game Over");
//            Time.timeScale = 0f; // Pause the game
            // Game over logic here
            if (collision.transform.rotation.z == 0)
                collision.transform.Rotate(0, 0, 90);
            collision.gameObject.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
            collision.gameObject.GetComponent<PlayerMovement>().enabled = false;
            gameOverUI.SetActive(true);
        }
//        Destroy(collision.gameObject);
    }
}
