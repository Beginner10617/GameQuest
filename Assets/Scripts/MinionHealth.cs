using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinionHealth : MonoBehaviour
{
    [SerializeField] GameObject bossHealthBarSprite;
    [SerializeField] string healthBarName = "empty health bar";
    float maxHealthScaleX, maxHealth;
    public int health = 500;
    public GameObject deathEffect;

    public bool isInvulnerable = false;
    public GameObject bossBoundaryWalls;

    [SerializeField] private GameObject decreaseEffectPrefab; // Prefab for the effect
    [SerializeField] public RectTransform effectSpawnPoint;
    [SerializeField] private Animator animator;
    [SerializeField] private LayerMask playerLayer;
    private BoxCollider2D _collider;
    public float timeToIncrease = 30f;

    public static event System.Action<MinionHealth> OnMinionDied;
    private void Start()
    {
        if (_collider == null) _collider = GetComponent<BoxCollider2D>();
        maxHealth = health;
        if (bossHealthBarSprite == null)
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                if (transform.GetChild(i).name == healthBarName)
                {
                    try
                    {
                        bossHealthBarSprite = transform.GetChild(i).GetChild(0).gameObject;
                    }
                    catch
                    {
                        Debug.LogError("Boss Health Bar name is not assigned as intended.");
                        return;
                    }
                    break;
                }
            }
        }
        if (bossHealthBarSprite == null)
        {
            Debug.LogError("Boss Health Bar not found. Please assign the health bar sprite.");
            return;
        }
        maxHealthScaleX = bossHealthBarSprite.transform.localScale.x;
    }
    void Update()
    {
        if (bossHealthBarSprite == null)
            return;
        Vector3 newScale = bossHealthBarSprite.transform.localScale;
        newScale.x = maxHealthScaleX * (health / maxHealth);
        bossHealthBarSprite.transform.localScale = newScale;
    }
    public void TakeDamage(int damage)
    {
        if (isInvulnerable)
            return;

        health -= damage;

        //if (health <= 200)
        //{
        //    GetComponent<Animator>().SetBool("IsEnraged", true);
        //}

        if (health <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        OnMinionDied?.Invoke(this);
        //Instantiate(deathEffect, transform.position, Quaternion.identity);
        //Destroy(gameObject);
        animator.SetBool("IsConvinced", true);
        isInvulnerable = true;
        _collider.excludeLayers += playerLayer;
        gameTimer.currentTime += timeToIncrease;
        SpawnIncreaseEffect();
        //bossBoundaryWalls.SetActive(false);
    }

    void SpawnIncreaseEffect()
    {
        if (decreaseEffectPrefab != null && effectSpawnPoint != null)
        {
            GameObject effect = Instantiate(decreaseEffectPrefab, effectSpawnPoint);
            RectTransform rectTransform = effect.GetComponent<RectTransform>();
            effect.GetComponent<DecreaseEffect>().timeToDecrease = timeToIncrease; // Set the time to decrease
            rectTransform.anchoredPosition = Vector2.zero;  // Set to center
            rectTransform.localScale = Vector3.one;
            effect.GetComponent<DecreaseEffect>().text = "+" + timeToIncrease.ToString() + "s";
        }
    }

}