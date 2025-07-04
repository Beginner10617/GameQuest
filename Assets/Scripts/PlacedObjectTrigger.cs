using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacedObjectTrigger : MonoBehaviour
{
    [SerializeField]
    private PlacingPairs placingPairs;
    private bool isTriggered = false;
    [SerializeField]
    private AudioClip soundEffect;
    private AudioSource audioSource;
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }
    void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Draggable"))
        {
            if(placingPairs.distortedObjects.IndexOf(other.gameObject) == placingPairs.placedObjects.IndexOf(gameObject))
            {
                other.gameObject.SetActive(false);
                gameObject.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1f);
                if (soundEffect != null && audioSource != null)
                {
                    audioSource.PlayOneShot(soundEffect);
                }
                isTriggered = true;
            }
        }
    }
    void Update()
    {
        if (isTriggered)
        {
            gameObject.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1f);
        }
    }
}
