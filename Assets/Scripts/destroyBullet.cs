using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class destroyBullet : MonoBehaviour
{
    public int bulletDamage = 15;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {

            collision.gameObject.GetComponent<BossHealth>().TakeDamage(bulletDamage);

        }
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ignore Collision"))
        {   Debug.Log("Ignoring collision with: " + collision.gameObject.name); 
            return;
        }

        Debug.Log("Bullet hit: " + collision.gameObject.name);
        Destroy(gameObject);
    }
}
