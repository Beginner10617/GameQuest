using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro; // If using TextMeshPro

public class DialogueManager : MonoBehaviour
{
    public GameObject dialoguePanel; // The UI panel for dialogue
    public TMP_Text dialogueText; // Reference to the text component (use Text if not using TMP)
    public Button nextButton; // Button to advance dialogue
    public SpawnEnemy _spawnEnemy;
    private string[] sentences;
    private int index = 0;

    public void StartDialogue(Dialogue dialogue)
    {
        dialoguePanel.SetActive(true); // Show dialogue panel
        sentences = dialogue.sentences;
        index = 0;
        dialogueText.text = sentences[index];
        //nextButton.gameObject.SetActive(true);

    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            ShowNextSentence();
        }
    }
    public void ShowNextSentence()
    {
        if (index < sentences.Length)
        {
            dialogueText.text = sentences[index];
            index++;
        }
        else
        {
            EndDialogue();
        }
    }

    private void EndDialogue()
    {
        dialoguePanel.SetActive(false); // Hide dialogue panel
        _spawnEnemy.Spawn();
    }
}
