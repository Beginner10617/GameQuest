using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ammo : MonoBehaviour
{
    private Rigidbody2D rb;
    [SerializeField] private Vector2 direction;
    [SerializeField] private float travelSpeed;
    [SerializeField] private float lifeTime = 2f;
    
    public void SetDirection(Vector2 endPoint)
    {
        direction = endPoint - (Vector2)transform.position;
        direction.Normalize();
    }
    public void SetSpeed(float speed)
    {
        travelSpeed = speed;
    }
    public void SetLifetime(float time)
    {
        lifeTime = time;
    }
    public void Shoot()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = direction * travelSpeed;
        Destroy(gameObject, lifeTime);
    }
}
