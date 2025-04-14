using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class gameTimer : MonoBehaviour
{
    [SerializeField] private Image timerImage;
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private float totalTime;
    [SerializeField] private bool restartTimerOnStart = false;
    public static float currentTime;
    public bool isTimerEnabled = true;
    // Start is called before the first frame update
    void Start()
    {
        if(restartTimerOnStart)
        {
            currentTime = totalTime;
        }
        timerText.text = currentTime.ToString();
        if(isTimerEnabled) StartCoroutine(TimerUpdater());
    }
    public void DisableTimer()
    {
        isTimerEnabled = false;
    }
    public void EnableTimer()
    {
        isTimerEnabled = true;
        Start();
    }
    public IEnumerator TimerUpdater()
    {
        while(currentTime > 0f)
        {
            timerImage.fillAmount = Mathf.InverseLerp(0, totalTime, currentTime);
            timerText.text = currentTime.ToString();
            yield return new WaitForSeconds(1f);
            currentTime--;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(currentTime < 0f)
        {
            timerImage.fillAmount = 0f;
            timerText.text = "0";
            Time.timeScale = 0f;
            Debug.Log("Game Over");
        }
    }
}
