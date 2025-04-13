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

    [SerializeField] private GameObject decreaseEffectPrefab; // Prefab for the effect
    [SerializeField] public RectTransform effectSpawnPoint;

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
        if (PlayerInSight()) { gameTimer.currentTime -= damage; SpawnDecreaseEffect(); }
        //_enemyPatrol.isAttacking = false;   

    }
    void SpawnDecreaseEffect()
    {
        if (decreaseEffectPrefab != null && effectSpawnPoint != null)
        {
            GameObject effect = Instantiate(decreaseEffectPrefab, effectSpawnPoint);
            RectTransform rectTransform = effect.GetComponent<RectTransform>();
            //effect.GetComponent<DecreaseEffect>().timeToDecrease = timeToDecrease; // Set the time to decrease
            rectTransform.anchoredPosition = Vector2.zero;  // Set to center
            rectTransform.localScale = Vector3.one;
            effect.GetComponent<DecreaseEffect>().text = "-" + damage.ToString() + "s";
        }
    }

}
