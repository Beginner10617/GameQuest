using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Interaction : MonoBehaviour
{
    [SerializeField]
    private GameObject dialoguePanel;
    [SerializeField]
    public TextMeshProUGUI dialogueText;
    [SerializeField]
    public string[] dialogue;
    public Texture[] speakerImage;
    public string[] speaker;
    [SerializeField]
    public float wordSpeed;
    [SerializeField]
    private bool playerClose;
    public RawImage rawImage;
    public TMP_Text speakerName;

    private int index;
    // Start is called before the first frame update
    void Start()
    {
        dialoguePanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        

    }

    public void zeroText()
    {
        StopAllCoroutines();
        dialogueText.text = "";
        index = 0;
        
        dialoguePanel.SetActive(false);
    }
    public void NextLine()
    {
        if (index < dialogue.Length - 1)
        {
            index++;
            dialogueText.text = "";
            StartCoroutine(Typing());
        }
        else
        {
            zeroText();
        }
    }

    IEnumerator Typing()
    {
        //istyping = true;
        rawImage.texture = speakerImage[index];
        Debug.Log(index);
        speakerName.text = speaker[index];
        int i = index;
        foreach (char letter in dialogue[index].ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(wordSpeed);
            if (i != index)
            {
                break;
            }
        }

        //istyping = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerClose = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerClose = false;
            zeroText();
        }
    }
}
