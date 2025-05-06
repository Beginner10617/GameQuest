using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
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
    [SerializeField] private AudioClip backgroundMusic;
    [SerializeField] private AudioMixerGroup audioMixerGroup;
    private AudioSource audioSource;
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (backgroundMusic != null)
        {
            audioSource.clip = backgroundMusic;
            audioSource.Play();
        }
        for (int i = 0; i < storyboards.Length; i++)
        {
            storyboards[i].SetActive(false);
        }
    }
    public void Skip()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(nextSceneName);
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
        Debug.Log(currentIndex);
        if (currentIndex < storyboards.Length)
        {
            GameObject previous = null;
            GameObject next = storyboards[currentIndex];
            if (currentIndex > 0)
            {
                previous = storyboards[currentIndex - 1];
                StartCoroutine(FadeCanvasGroupAlpha(previous, null, false));
            }
            StartCoroutine(FadeCanvasGroupAlpha(next, previous, true));
            currentIndex++;
        }
        else
        {
            // Load the next scene
            UnityEngine.SceneManagement.SceneManager.LoadScene(nextSceneName);
        }
    }

    IEnumerator FadeCanvasGroupAlpha(GameObject obj, GameObject previous, bool fadeIn)
    {
        Graphic graphic = obj.GetComponent<Graphic>();
        if (graphic == null)
            yield break;

        float startAlpha = fadeIn ? 0f : 1f;
        float endAlpha = fadeIn ? 1f : 0f;

        // Set initial alpha for fade-in
        if (fadeIn)
        {
            Color newColor = graphic.color;
            newColor.a = 0f;
            graphic.color = newColor;
        }
        else
        {
            Color newColor = graphic.color;
            newColor.a = 1f;
            graphic.color = newColor;
        }
        if (previous != null)
        {
            // Wait for the previous storyboard to finish fading out
            while (previous.activeSelf)
            {
                yield return null;
            }
        }
        obj.SetActive(true);
        float timer = 0f;
        while (timer < fadeDuration)
        {
            float alpha = Mathf.Lerp(startAlpha, endAlpha, timer / fadeDuration);
            Color newColor = graphic.color;
            newColor.a = alpha;
            graphic.color = newColor;
            if(obj == storyboards[storyboards.Length - 1])
            {
                // Set the audio mixer group volume based on the alpha value
                float volume = Mathf.Lerp(0, -80f, alpha);
                audioMixerGroup.audioMixer.SetFloat("Volume", volume);
            }
            timer += Time.deltaTime;
            yield return null;
        }

        // Ensure final alpha is set
        Color finalColor = graphic.color;
        finalColor.a = endAlpha;
        graphic.color = finalColor;
        if (!fadeIn)
        {
            obj.SetActive(false);
        }
    }
}
