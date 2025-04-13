using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMukka : MonoBehaviour
{
    public float timeInBetweenMukka = 1f;
    public Animator animator;
    private float timer = 0f;

    [SerializeField] private int damage;
    [SerializeField] private float range;
    [SerializeField] private float shiftUp;
    [SerializeField] private float height;
    [SerializeField] private float colliderSize;
    [SerializeField] private BoxCollider2D _collider;
    [SerializeField] private LayerMask enemyLayer;

    BossHealth _bossHealth;
    void FixedUpdate()
    {
        timer += Time.deltaTime;
        //Debug.LogWarning(timer);
        if(InputManager.mukkaPressed > 0 && timer >= timeInBetweenMukka)
        {
            //Debug.LogWarning("YOOOOOOOOOO");
            animator.SetTrigger("Attack");
            timer = 0;
        }
    }


    private bool EnemyInSight()
    {
        RaycastHit2D hit = Physics2D.BoxCast(_collider.bounds.center + transform.right * range * transform.localScale.x + transform.up * shiftUp,
            new Vector3(_collider.bounds.size.x * colliderSize, _collider.bounds.size.y * height, _collider.bounds.size.z),
            0, Vector2.left, 0, enemyLayer);
        
        hit.collider.gameObject.TryGetComponent<BossHealth>(out _bossHealth);
        return hit.collider != null;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(_collider.bounds.center + transform.right * range * transform.localScale.x + transform.up * shiftUp,
            new Vector3(_collider.bounds.size.x * colliderSize, _collider.bounds.size.y * height, _collider.bounds.size.z));
    }

    public void DoDamage()
    {

        if (EnemyInSight())
        {
            //Debug.LogWarning("Player Mukka marr raha h");
            if(_bossHealth != null) _bossHealth.TakeDamage(damage);
        }
        //_enemyPatrol.isAttacking = false;   

    }
}
