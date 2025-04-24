using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ClipBoard : MonoBehaviour
{
    [SerializeField]
    private DragAndDrop dragAndDropHandler;
    [SerializeField]
    public GameObject clipedObject;
    private bool colliderIsActive = false;
    
    private void Awake()
    {
        dragAndDropHandler = FindObjectOfType<DragAndDrop>();
    }
    void Start()
    {
        GetComponent<Collider2D>().enabled = colliderIsActive;   
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Draggable") && clipedObject == null)
        {
            clipedObject = other.gameObject;
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Draggable") && clipedObject == other.gameObject)
        {
            clipedObject = null;
        }
    }
    void Update()
    {
        if(clipedObject != null && !colliderIsActive)
        {
            colliderIsActive = true;
            GetComponent<Collider2D>().enabled = colliderIsActive;
        }
        if(clipedObject == null && colliderIsActive)
        {
            colliderIsActive = false;
            GetComponent<Collider2D>().enabled = colliderIsActive;
        }
        if(dragAndDropHandler.selectedObject == clipedObject && colliderIsActive)
        {
            colliderIsActive = false;
            GetComponent<Collider2D>().enabled = colliderIsActive;
        }
    }
}
