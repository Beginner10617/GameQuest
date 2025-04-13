using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagePlayer : MonoBehaviour
{
    public float bulletDamage = 15f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {

            gameTimer.currentTime -= bulletDamage;

        }
        else if (collision.gameObject.tag == "Enemy")
        {
            return;
        }
        Destroy(gameObject);
    }
}
