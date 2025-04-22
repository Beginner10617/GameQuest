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
    public GameObject player;
    [SerializeField]
    private bool AutoType;
    [SerializeField]
    private float autoTypeWaitTime;
    public RawImage rawImage;
    public TMP_Text speakerName;
    public bool stopPlayer;

    private int index;
    // Start is called before the first frame update
    void Start()
    {
        index = 0;
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
        player.GetComponent<PlayerMovement>().enabled = true;
        GetComponent<BoxCollider2D>().enabled = false;
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
        try{
            rawImage.texture = speakerImage[index];
        }
        catch
        {
            Debug.Log("No image found");
        }
        Debug.Log(index);
        try{
            speakerName.text = speaker[index];
        }
        catch
        {
            Debug.Log("No speaker found");
        }
        int i = index;
        foreach (char letter in dialogue[index].ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(1/wordSpeed);
            if (i != index)
            {
                break;
            }
        }
        if (AutoType)
        {
            yield return new WaitForSeconds(autoTypeWaitTime);
            NextLine();
        }
        //istyping = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            dialoguePanel.SetActive(true);
            player.GetComponent<PlayerMovement>().animator.SetFloat("Speed", 0);
            player.GetComponent<PlayerMovement>().rb.velocity = Vector2.zero;
            player.GetComponent<PlayerMovement>().enabled = false;
            index = -1;
            NextLine();
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            dialoguePanel.SetActive(false);
            zeroText();
        }
    }
}
