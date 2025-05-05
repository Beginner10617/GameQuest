using TMPro;
using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    public Transform shootPosition;
    public GameObject bullet;
    public float bulletSpeed = 50f;
    public float timeInBetweenBullets = 0.1f;
    [SerializeField] private GameObject StoneUI;
    [SerializeField] private TextMeshProUGUI ammoUI;
    [SerializeField] private int ammo = 10;

    private float timer = 0f;
    void Start()
    {
    //    Debug.Log("Time left:" + gameTimer.currentTime.ToString());
        if(ammoUI != null) ammoUI.text = ammo.ToString();
    }
    private void OnEnable()
    {
        StoneUI.SetActive(true);
    }
    void FixedUpdate()
    {
        timer += Time.deltaTime;

        if (timer >= timeInBetweenBullets && InputManager.shootIsHeld > 0 && ammo > 0)
        {
            GameObject b = Instantiate(bullet, shootPosition.position, Quaternion.identity);
            ammo--;
            if (ammoUI != null) ammoUI.text = ammo.ToString();
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
