using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class destroyBullet : MonoBehaviour
{
    public float bulletDamage = 15f;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            
            gameTimer.currentTime -= bulletDamage;

        }
        else if(collision.gameObject.tag == "Enemy")
        {
            collision.gameObject.GetComponent<BossHealth>().TakeDamage(30);
            Debug.LogWarning(collision.gameObject.GetComponent<BossHealth>().health);
        }
        Destroy(gameObject);
    }
}
