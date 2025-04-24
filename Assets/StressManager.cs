using UnityEngine;
using UnityEngine.UI;

public class StressManager : MonoBehaviour
{
    public float stressLevel = 30f;
    public float maxStressLevel = 100f;
    public ClipBoard[] clipBoard;
    private Slider stressSlider;
    public GameObject[] gameOverObjects;
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
        foreach (var item in clipBoard)
        {
            if(item.clipedObject != null)
            {
                stressLevel += Time.deltaTime * item.clipedObject.GetComponent<Task>().stressChange;
            }
        }
    }
}
