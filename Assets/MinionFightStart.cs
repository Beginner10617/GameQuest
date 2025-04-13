using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinionFightStart : MonoBehaviour
{
    [SerializeField] private List<Animator> animator;
    [SerializeField] private GameObject boundary;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            foreach (Animator anim in animator)
            {
                anim.SetTrigger("StartChasing");
            }
            
            //boundaries[0].SetActive(false);
            boundary.SetActive(true);
        }
    }
}
