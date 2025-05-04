using UnityEngine;
using UnityEngine.UI;
using System.Collections;
public class storyboard : MonoBehaviour
{
    [Header("Storyboard Settings")]
    [SerializeField] private GameObject[] storyboards;
    [SerializeField] private float[] timeGaps;
    [SerializeField] private float timeGap = 5f;
    [SerializeField] private bool constantTimeGap = true;
    [SerializeField] private float fadeDuration = 1f;
    private float timer = 0;
    private int currentIndex = 0;
    [SerializeField] private string nextSceneName;
    
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < storyboards.Length; i++)
        {
            storyboards[i].SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(timer <= 0)
        {
            NextStoryboard();
            if(!constantTimeGap)
            {
                timeGap = timeGaps[currentIndex];
            }
            timer = timeGap;
        }
        else
        {
            timer -= Time.deltaTime;
        }
    }
    void NextStoryboard()
    {
        if (currentIndex < storyboards.Length)
        {
            GameObject next = storyboards[currentIndex];
            if (currentIndex > 0)
            {
                GameObject previous = storyboards[currentIndex - 1];
                StartCoroutine(FadeCanvasGroupAlpha(previous, false));
            }
            StartCoroutine(FadeCanvasGroupAlpha(next, true));
            currentIndex++;
        }
        else
        {
            // Load the next scene
            UnityEngine.SceneManagement.SceneManager.LoadScene(nextSceneName);
        }
    }

    IEnumerator FadeCanvasGroupAlpha(GameObject obj, bool fadeIn)
    {
        Graphic graphic = obj.GetComponent<Graphic>();
        if (graphic == null)
            yield break;

        Color originalColor = graphic.color;
        float startAlpha = fadeIn ? 0f : originalColor.a;
        float endAlpha = fadeIn ? originalColor.a : 0f;

        // Set initial alpha for fade-in
        if (fadeIn)
        {
            Color newColor = originalColor;
            newColor.a = 0f;
            graphic.color = newColor;
            obj.SetActive(true);
        }

        float timer = 0f;
        while (timer < fadeDuration)
        {
            float alpha = Mathf.Lerp(startAlpha, endAlpha, timer / fadeDuration);
            Color newColor = originalColor;
            newColor.a = alpha;
            graphic.color = newColor;
            timer += Time.deltaTime;
            yield return null;
        }

        // Ensure final alpha is set
        Color finalColor = originalColor;
        finalColor.a = endAlpha;
        graphic.color = finalColor;
        if (!fadeIn)
        {
            obj.SetActive(false);
        }
    }
}
