using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private int lifes = 3;
    [SerializeField] private GameObject gameOverScreen;
    [SerializeField] private TMPro.TextMeshProUGUI lifeText;
    private void Start()
    {
        lifeText.text = "x" + lifes.ToString();
        gameOverScreen.SetActive(false);
    }
    public void Damage()
    {
        lifes--;
        lifeText.text = "x" + lifes.ToString();
        if (lifes <= 0)
        {
            gameOverScreen.SetActive(true);
            Time.timeScale = 0;
        }
    }

}
