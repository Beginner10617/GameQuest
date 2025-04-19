using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    public Transform shootPosition;
    public GameObject bullet;
    public float bulletSpeed = 50f;
    public float timeInBetweenBullets = 0.1f;

    private float timer = 0f;
    void Start()
    {
    //    Debug.Log("Time left:" + gameTimer.currentTime.ToString());
    }
    void FixedUpdate()
    {
        timer += Time.deltaTime;

        if (timer >= timeInBetweenBullets && InputManager.shootIsHeld > 0)
        {
            GameObject b = Instantiate(bullet, shootPosition.position, Quaternion.identity);
            Rigidbody2D rb = b.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.velocity = shootPosition.right * bulletSpeed;
            }
            timer = 0;
            if(b != null) Destroy(b, 10f);
        }
    }
}
