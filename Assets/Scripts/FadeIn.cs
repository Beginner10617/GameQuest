using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;
using UnityEngine.SceneManagement;

public class SceneFader : MonoBehaviour
{
    public Image fadeImage;
    public float fadeDuration = 1f;
    public TextMeshProUGUI txt;
    public TextMeshProUGUI txt2;
    
    public void StartFade()
    {
        StartCoroutine(FadeIn());
        StartCoroutine(FadeInTxt(txt));
    }

    IEnumerator FadeIn()
    {
        float elapsedTime = 0f;
        Color color = fadeImage.color;
        while (elapsedTime < fadeDuration)
        {
            float alpha = Mathf.Lerp(0f, 1f, elapsedTime / fadeDuration);
            fadeImage.color = new Color(color.r, color.g, color.b, alpha);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        fadeImage.color = new Color(color.r, color.g, color.b, 1f);
        
        //fadeImage.gameObject.SetActive(false); // Hide the image after fading out
    }
    IEnumerator FadeInTxt(TextMeshProUGUI t)
    {
        float elapsedTime = 0f;
        Color txtColor = t.color;
        while (elapsedTime < fadeDuration)
        {
            float alpha = Mathf.Lerp(0f, 1f, elapsedTime / fadeDuration);
            t.alpha = alpha;
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        t.alpha = 1;
        if(txt2.alpha != 1) StartCoroutine(FadeInTxt(txt2));
        else if(txt2.alpha == 1)
        {
            yield return new WaitForSeconds(3.5f);
            SceneManager.LoadScene("MainMenu");
        }
    }

}