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
    [SerializeField]
    public bool disableAfterInteraction;
    public RawImage rawImage;
    public TMP_Text speakerName;
    public bool stopPlayer;
    [SerializeField]
    private GameObject[] enableObjectsAfterInteraction;
    [SerializeField]
    private GameObject[] disableObjectsBeforeInteraction;
    private int index;
    private bool istyping = false;
    public bool isPowerUpInteraction = false;
    [SerializeField] private GameObject PowerUpMessage;
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
    public void ManuallyTrigger()
    {
            foreach (GameObject obj in disableObjectsBeforeInteraction)
            {
                obj.SetActive(false);
            }
            StartDialogues();
        
    }
    IEnumerator PowerUpMessageObject()
    {
        PowerUpMessage.SetActive(true);
        player.GetComponent<PlayerShoot>().enabled = true;
        yield return new WaitForSeconds(3f);
        PowerUpMessage.SetActive(false);
    }

    public void zeroText()
    {
        StopAllCoroutines();
        if(isPowerUpInteraction)
        {
            if(PowerUpMessage != null) StartCoroutine(PowerUpMessageObject());
        }
        dialogueText.text = "";
        index = 0;
        if(gameObject.TryGetComponent(out ObjectsFallingGameStart starter))
        {
            starter.startGame();
        }
        dialoguePanel.SetActive(false);
        player.GetComponent<PlayerMovement>().enabled = true;
        player.GetComponent<PlayerShoot>().enabled = true;
        if (disableAfterInteraction)
            GetComponent<BoxCollider2D>().enabled = false;
        foreach (GameObject obj in enableObjectsAfterInteraction)
        {
            obj.SetActive(true);
        }
    }
    public void NextLine()
    {
        if(istyping)
        {
            istyping = false;
            dialogueText.text = dialogue[index];
            return;
        }
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
        istyping = true;
        try{
            rawImage.texture = speakerImage[index];
        }
        catch
        {
            //Debug.Log("No image found");
        }
        //Debug.Log(index);
        try{
            speakerName.text = speaker[index];
        }
        catch
        {
            //Debug.Log("No speaker found");
        }
        int i = index;
        foreach (char letter in dialogue[index])
        {
            if(istyping == false)
            {
                istyping = true;
                break;
            }
            dialogueText.text += letter;
            yield return new WaitForSeconds(1/wordSpeed);
            if (i != index)
            {
                break;
            }
        }
        if (AutoType && istyping)
        {
            yield return new WaitForSeconds(autoTypeWaitTime);
            istyping = false;
            NextLine();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            foreach (GameObject obj in disableObjectsBeforeInteraction)
            {
                obj.SetActive(false);
            }
            StartDialogues();
        }
    }
    public void StartDialogues()
    {
        dialoguePanel.SetActive(true);
        player.GetComponent<PlayerMovement>().animator.SetFloat("Speed", 0);
        player.GetComponent<PlayerMovement>().rb.velocity = Vector2.zero;
        player.GetComponent<PlayerMovement>().enabled = false;
        player.GetComponent<PlayerShoot>().enabled = false;
        index = -1;
        NextLine();
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            dialoguePanel.SetActive(false);
            zeroText();
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        OnTriggerEnter2D(collision.collider);
    } 
    private void OnCollisionExit2D(Collision2D collision)
    {
        OnTriggerExit2D(collision.collider);
    }
}