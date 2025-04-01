using UnityEditor;
using UnityEngine;

public class princess : MonoBehaviour
{
    [SerializeField] private LayerMask playerLayer;
    [Header("Movement")]
    [SerializeField] private bool patrolling = true;
    [SerializeField] private float speed = 2f;
    [SerializeField] Transform startPoint;
    [SerializeField] Transform endPoint;
    [SerializeField] private int direction = 1;
    [Header("Attack")]
    [SerializeField] bool attacking = false;
    [SerializeField] private float attackRange;
    [SerializeField] private float attackCooldown;
    private bool canAttack = true;
    [Header("Ammo")]
    [SerializeField] GameObject ammoPrefab;
    [SerializeField] Transform ammoSpawnPoint;
    [SerializeField] private float ammoSpeed;
    [SerializeField] private float ammoLifeTime;
    [SerializeField] private float ammoDamage;
    [Header("Decrease Effect")]
    [SerializeField] private RectTransform effectSpawnPoint;
    void Start()
    {
        direction = (int)Mathf.Sign(endPoint.position.x - startPoint.position.x);
        Debug.Log("Direction: " + direction.ToString());
    }

    void Update()
    {
        CheckAttack();
        if (patrolling)
        {
            Patrol();
        }
        else if (attacking)
        {
            AttackPlayer();
        }
    }
    private void CheckAttack()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, attackRange, playerLayer);
        if (colliders.Length > 0)
        {
            attacking = true;
            patrolling = false;
            //Debug.Log("Attacking");
            return;
        }
        attacking = false;
        patrolling = true;
        //Debug.Log("Patrolling");
    }
    private void Patrol()
    {
        transform.localScale = new Vector3(-direction * Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        transform.position += new Vector3(speed * Time.deltaTime * direction, 0, 0);
        if (transform.position.x >= endPoint.position.x)
        {
            direction = -1;
        }
        else if (transform.position.x <= startPoint.position.x)
        {
            direction = 1;
        }
    }
    private void AttackPlayer()
    {
        if (attacking && canAttack)
        {
            Collider2D player = Physics2D.OverlapCircle(transform.position, attackRange, playerLayer);
            if (player != null)
            {
                Vector2 directionToPlayer = (player.transform.position - ammoSpawnPoint.position).normalized;
                transform.localScale = new Vector3(-Mathf.Sign(directionToPlayer.x) * Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
                //Debug.LogWarning(directionToPlayer);
                GameObject ammo = Instantiate(ammoPrefab, ammoSpawnPoint.position, Quaternion.identity);
                ammo.GetComponent<decreaseTimer>().effectSpawnPoint = effectSpawnPoint;
                Rigidbody2D ammoRb = ammo.GetComponent<Rigidbody2D>();
                if (ammoRb != null)
                {
                    ammoRb.velocity = new Vector2(directionToPlayer.x * ammoSpeed, directionToPlayer.y * ammoSpeed);
                }
                Destroy(ammo, ammoLifeTime);
                canAttack = false;
                Invoke(nameof(ResetAttack), attackCooldown);
            }
        }
    }
    private void ResetAttack()
    {
        canAttack = true;
    }
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red; // Set color for visibility
#if UNITY_EDITOR
        Handles.color = Color.red;
        Handles.DrawWireDisc(transform.position, Vector3.forward, attackRange);
#endif
    }
}
