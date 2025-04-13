using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateLvl3Cutscene : MonoBehaviour
{
    public Transform destPos;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            CutsceneManager.isCutSceneRunning = true ;
            this.gameObject.SetActive(false) ;
        }
    }
}
