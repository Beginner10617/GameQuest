using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class decreaseTimer : MonoBehaviour
{
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            gameTimer.currentTime -= 20f;
            Destroy(gameObject);
        }
        
    }
}
