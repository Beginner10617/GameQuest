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
    [Header("Ammo")]
    [SerializeField] GameObject ammoPrefab;
    [SerializeField] Transform ammoSpawnPoint;
    [SerializeField] private float ammoSpeed;
    [SerializeField] private float ammoLifeTime;
    [SerializeField] private float ammoDamage;
    void Start()
    {
        direction = (int) Mathf.Sign(endPoint.position.x - startPoint.position.x);
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
           // Attack();
        }   
    }
    private void CheckAttack()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, attackRange, playerLayer);
        if(colliders.Length > 0)
        {
            attacking = true;
            patrolling = false;
            Debug.Log("Attacking");
            return;
        }
        attacking = false;
        patrolling = true;
        Debug.Log("Patrolling");
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
    void OnDrawGizmosSelected()
{
    Gizmos.color = Color.red; // Set color for visibility
#if UNITY_EDITOR
    Handles.color = Color.red;
    Handles.DrawWireDisc(transform.position, Vector3.forward, attackRange);
#endif
}
}
