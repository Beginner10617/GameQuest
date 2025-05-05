using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class startBossFight1 : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject boundary;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            animator.SetTrigger("StartBossFight");
            //boundaries[0].SetActive(false);
            boundary.SetActive(true);
            gameObject.SetActive(false);
        }
    }
}
