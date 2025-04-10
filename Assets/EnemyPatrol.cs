using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPatrolAndChase : MonoBehaviour
{
    [Header("Patrol Points")]
    [SerializeField] private Transform leftEdge;
    [SerializeField] private Transform rightEdge;

    [Header("Movement")]
    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private Transform enemy;

    [Header("Chase Settings")]
    [SerializeField] private float chaseRange = 5f;
    [SerializeField] private float stopDistance = 1.2f;

    private Transform player;
    private Vector3 initScale;
    private bool movingLeft = true;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        initScale = enemy.localScale;
    }

    private void Update()
    {
        if (player == null) return;

        float distanceToPlayer = Mathf.Abs(enemy.position.x - player.position.x);

        if (distanceToPlayer <= chaseRange && distanceToPlayer > stopDistance)
        {
            ChasePlayer();
        }
        else if (distanceToPlayer > chaseRange)
        {
            Patrol();
        }
        // else: If within stopDistance, just idle
    }

    private void ChasePlayer()
    {
        float direction = Mathf.Sign(player.position.x - enemy.position.x);
        Vector3 targetPosition = new Vector3(player.position.x, enemy.position.y, enemy.position.z);
        enemy.position = Vector3.MoveTowards(enemy.position, targetPosition, moveSpeed * Time.deltaTime);

        if (direction != 0)
            enemy.localScale = new Vector3(Mathf.Abs(initScale.x) * direction, initScale.y, initScale.z);
    }

    private void Patrol()
    {
        if (movingLeft)
        {
            if (enemy.position.x > leftEdge.position.x)
            {
                MoveInDirection(-1);
            }
            else
            {
                movingLeft = false;
            }
        }
        else
        {
            if (enemy.position.x < rightEdge.position.x)
            {
                MoveInDirection(1);
            }
            else
            {
                movingLeft = true;
            }
        }
    }

    private void MoveInDirection(int direction)
    {
        enemy.localScale = new Vector3(Mathf.Abs(initScale.x) * direction, initScale.y, initScale.z);
        enemy.position = new Vector3(enemy.position.x + Time.deltaTime * direction * moveSpeed,
                                     enemy.position.y, enemy.position.z);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, chaseRange);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, stopDistance);
    }
}
