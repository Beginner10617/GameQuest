using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class meleeEnemy : MonoBehaviour
{
    [SerializeField] private float attackCooldown;
    [SerializeField] private int damage;
    [SerializeField] private float range;
    [SerializeField] private float colliderSize;
    [SerializeField] private BoxCollider2D _collider;
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private Animator animator;
    [SerializeField] EnemyPatrol _enemyPatrol;
    private float cooldownTimer = Mathf.Infinity;
    public bool animRunning = false;
    private void Update()
    {
        cooldownTimer += Time.deltaTime;

        if (PlayerInSight())
        {
            Debug.Log("PLAYER SIGHTED");
            if (cooldownTimer >= attackCooldown)
            {
                cooldownTimer = 0;
                animator.SetTrigger("Attack");
                
                Debug.Log("Attacked Player");
                

            }
        }
        
        
    }

    public void AttackAnimStarted()
    {
        animator.SetFloat("moveSpeed", 0f);
        animRunning = true;
    }
    public void attackAnimCompleted()
    {
        animRunning = false;
    }
    private bool PlayerInSight()
    {
        RaycastHit2D hit = Physics2D.BoxCast(_collider.bounds.center + transform.right * range * transform.localScale.x, 
            new Vector3(_collider.bounds.size.x * colliderSize, _collider.bounds.size.y, _collider.bounds.size.z),
            0, Vector2.left, 0, playerLayer);
        return hit.collider != null;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(_collider.bounds.center + transform.right * range * transform.localScale.x, 
            new Vector3(_collider.bounds.size.x * colliderSize, _collider.bounds.size.y, _collider.bounds.size.z));
    }

    public void ReduceTimer(int time)
    {
        if (PlayerInSight()) { gameTimer.currentTime -= time; }
        //_enemyPatrol.isAttacking = false;   

    }
}

