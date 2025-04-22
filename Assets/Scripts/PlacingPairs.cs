using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlacingPairs : MonoBehaviour
{
    [SerializeField]
    public List<GameObject> distortedObjects = new List<GameObject>();
    [SerializeField]
    public List<GameObject> placedObjects = new List<GameObject>();
    [SerializeField]
    private DragAndDrop dragDropHandler;
    [SerializeField]
    private GameObject nextStepLock;

    void Start()
    {
        nextStepLock.SetActive(true);
        for (int i = 0; i < distortedObjects.Count; i++)
        {
            placedObjects[i].GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0f);
        }   
    }
    void Update()
    {      
        if(dragDropHandler.selectedObject != null)
        {
            int index = distortedObjects.IndexOf(dragDropHandler.selectedObject);
            if (index != -1)
            {
                GameObject placedObject = placedObjects[index];
                if (placedObject != null)
                {
                    placedObject.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0.5f);
                }
            }
        }  
        bool allInactive = true;
        foreach(GameObject distortedObject in distortedObjects)
        {
            if (distortedObject.activeSelf)
            {
                allInactive = false;
                break;
            }
        }
        if (allInactive)
        {
            nextStepLock.SetActive(false);    
            gameObject.SetActive(false);    
        }
    }
}
