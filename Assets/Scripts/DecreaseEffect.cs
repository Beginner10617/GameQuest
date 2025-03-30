using UnityEngine;
using TMPro;
public class DecreaseEffect : MonoBehaviour
{
    [SerializeField] private float scaleSpeed = 0.1f; // Speed of scaling down
    [SerializeField] private float fadeSpeed = 0.1f; // Speed of fading out
    [SerializeField] private float moveSpeed = 0.5f; // Speed of moving the effect upwards
    [SerializeField] public float destroyDelay = 2f; // Delay before destroying the effect
    public string text;
    public float timeToDecrease = 5f; // Time to decrease the timer by 5 seconds
    private float timer = 0;
    void Update()
    {
        timer += Time.deltaTime;
        if (timer < destroyDelay)
        {
            // Scale down the effect
            transform.localScale -= new Vector3(scaleSpeed, scaleSpeed, 0) * Time.deltaTime;
            if (transform.localScale.x < 0)
            {
                transform.localScale = new Vector3(0, 0, 0); // Prevent negative scale
                timer = destroyDelay; // Set timer to destroyDelay to ensure it gets destroyed
            }
            // Fade out the effect
            Color color = GetComponent<TMP_Text>().color;
            color.a -= fadeSpeed * Time.deltaTime;
            if (color.a < 0)
            {
                timer = destroyDelay; // Set timer to destroyDelay to ensure it gets destroyed
            }
            GetComponent<TMP_Text>().color = color;

            // Move the effect upwards
            GetComponent<RectTransform>().anchoredPosition += new Vector2(0, moveSpeed) * Time.deltaTime;

            // Update the text
            GetComponent<TMP_Text>().text = text;

        }
        else
        {
        
            gameTimer.currentTime -= timeToDecrease;
            Debug.Log("Decreased time by " + timeToDecrease + " seconds. Current time: " + gameTimer.currentTime);
            Destroy(gameObject);
        }
    }

}
