using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Level22 : MonoBehaviour
{
    private GameObject player;
    private TimeSlot[] timeSlots;
    public GameObject[] ObjectEnableAfterEnd;
    public Button endLevelButton;
    void Start(){
        timeSlots = FindObjectsOfType<TimeSlot>();
    }
    void OnEnable()
    {
        player = GameObject.Find("Player");
        player.GetComponent<PlayerMovement>().enabled = false;
        endLevelButton.onClick.AddListener(EndLevel);
        endLevelButton.gameObject.SetActive(false);
    }
    void Update()
    {
        bool levelEnd = true;
        foreach(TimeSlot timeSlot in timeSlots)
        {
            if(timeSlot.isFilled == false)
            {
                levelEnd = false;
                break;
            }
        }
        if(levelEnd)
        {
            endLevelButton.gameObject.SetActive(true);
        }
        else
        {
            endLevelButton.gameObject.SetActive(false);
        }
    }
    public void EndLevel()
    {
        player.GetComponent<PlayerMovement>().enabled = true;
        foreach(GameObject obj in ObjectEnableAfterEnd)
        {
            obj.SetActive(true);
        }
        gameObject.SetActive(false);
    }

}
