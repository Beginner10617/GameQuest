using Unity.VisualScripting.Dependencies.Sqlite;
using UnityEngine;
using UnityEngine.UI;

public class StressManager : MonoBehaviour
{
    public float stressLevel = 30f;
    public float maxStressLevel = 100f;
    public ClipBoard[] clipBoard;
    private Slider stressSlider;
    public GameObject[] gameOverObjects;
    [SerializeField]
    private Interaction step2End;
    void Start()
    {
        clipBoard = FindObjectsOfType<ClipBoard>();
        stressSlider = GetComponent<Slider>();
    }
    
    private void Update()
    {
        stressSlider.value = stressLevel / maxStressLevel;
        if(stressLevel >= maxStressLevel)
        {
            foreach (var item in gameOverObjects)
            {
                item.gameObject.SetActive(true);
                Time.timeScale = 0;
            }
        }
        if (stressLevel < 0)
        {
            stressLevel = 0;
        }
        bool allTasksDone = true;
        bool educated = false;
        foreach (var item in clipBoard)
        {
            if(item.clipedObject != null)
            {
                if(item.clipedObject.GetComponent<Task>().isEducated)
                {
                    educated = true;
                }
                stressLevel += Time.deltaTime * item.clipedObject.GetComponent<Task>().stressChange;
            }
            else
            {
                allTasksDone = false;
            }
        }
        if(allTasksDone && !educated)
        {
            step2End.dialogue = new string[1];
            step2End.dialogue[0] = "That's why u get Fs";
            step2End.disableAfterInteraction = true;
        }
        else if(allTasksDone)
        {
            step2End.gameObject.SetActive(false);
        }
    }
}
