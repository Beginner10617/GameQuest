using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{
    [SerializeField] private float chaseRange = 5f;
    [SerializeField] private float stopDistance = 1.2f;
    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private Transform enemy;

    private Transform player;
    private Vector3 initScale;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        initScale = enemy.localScale;
    }

    private void Update()
    {
        if (player == null) return;

        float distanceToPlayer = Mathf.Abs(enemy.position.x - player.position.x); // Only X distance

        if (distanceToPlayer <= chaseRange && distanceToPlayer > stopDistance)
        {
            FollowPlayer();
        }
        else
        {
            StopMoving();
        }
    }

    private void FollowPlayer()
    {
        // Move only on X-axis
        float direction = Mathf.Sign(player.position.x - enemy.position.x);
        Vector3 targetPosition = new Vector3(player.position.x, enemy.position.y, enemy.position.z);
        enemy.position = Vector3.MoveTowards(enemy.position, targetPosition, moveSpeed * Time.deltaTime);

        // Flip enemy to face player
        if (direction != 0)
        {
            enemy.localScale = new Vector3(Mathf.Abs(initScale.x) * direction, initScale.y, initScale.z);
        }
    }

    private void StopMoving()
    {
        // Idle or do nothing
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, chaseRange);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, stopDistance);
    }
}
