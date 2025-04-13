using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAttack : MonoBehaviour
{
    [SerializeField] private int damage;
    [SerializeField] private float range;
    [SerializeField] private float colliderSize;
    [SerializeField] private BoxCollider2D _collider;
    [SerializeField] private LayerMask playerLayer;
    

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

    public void ReduceTimer()
    {
        if (PlayerInSight()) { gameTimer.currentTime -= damage; }
        //_enemyPatrol.isAttacking = false;   

    }

    
}
