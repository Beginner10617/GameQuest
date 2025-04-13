using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHealth : MonoBehaviour
{
    [SerializeField] GameObject bossHealthBarSprite;
    [SerializeField] string healthBarName = "empty health bar";
    float maxHealthScaleX, maxHealth;
    public int health = 500;
    public GameObject deathEffect;

    public bool isInvulnerable = false;
    public GameObject bossBoundaryWalls;

    private void Start()
    {
        maxHealth = health;
        if(bossHealthBarSprite == null)
        {
            for(int i = 0; i < transform.childCount; i++)
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
        //Instantiate(deathEffect, transform.position, Quaternion.identity);
        Destroy(gameObject);
        bossBoundaryWalls.SetActive(false);
    }

}