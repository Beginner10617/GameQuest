using UnityEngine;
using UnityEngine.UI;

public class StressManager : MonoBehaviour
{
    public static StressManager Instance;

    [Range(0, 100)] public int stressLevel = 40;
    public Slider stressSlider;
    public GameObject[] gameOverObjects;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        if (stressSlider != null)
        {
            stressSlider.minValue = 0;
            stressSlider.maxValue = 100;
            stressSlider.value = stressLevel;
        }
    }

    void Update()
    {
        if (stressSlider != null)
        {
            stressSlider.value = stressLevel;
            Color sliderColor = Color.Lerp(Color.green, Color.red, stressLevel / 100f);
            stressSlider.fillRect.GetComponent<Image>().color = sliderColor;
        }
        if (stressLevel >= 100)
        {
            foreach (GameObject obj in gameOverObjects)
            {
                obj.SetActive(true);
            }
            Time.timeScale = 0;
        }
        else
        {
            foreach (GameObject obj in gameOverObjects)
            {
                obj.SetActive(false);
            }
            Time.timeScale = 1;
        }
    }

    public void ApplyStressChange(int delta)
    {
        stressLevel = Mathf.Clamp(stressLevel + delta, 0, 100);
    }
}
