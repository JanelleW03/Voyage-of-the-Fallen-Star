using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    public GameObject dialogueBox;
    public TMP_Text textComponent;

    List<DialogueField> dialogueData;
    int dialogueIndex;
    //public TMP_Text dialogueText;
    int lineIndex;

    float charDelay = 0.1f;

    void Awake()
    {
        //x textComponent = GetComponentInChildren<TMP_Text>();
        //defaultSize = textComponent.fontSize;

        if (textComponent == null)
        {
            Debug.LogError("TMP_Text not found inside DialogueManager!");
        }

       // dialogueBox.SetActive(false);
    }

     void Start()
    {
        dialogueBox.SetActive(false);

    }

    public void StartDialogue(List<DialogueField> newDialogue)
    {
        dialogueBox.SetActive(true);

        dialogueData = newDialogue;
        dialogueIndex = 0;
        lineIndex = 0;

        ShowLine();
    }

    public void NextLine()
    {
        if (textComponent.maxVisibleCharacters < textComponent.text.Length)
        {
            textComponent.maxVisibleCharacters = textComponent.text.Length;
            return;
        }

        lineIndex++;

        if (lineIndex >= dialogueData[dialogueIndex].dialogue.Count)
        {
            dialogueBox.SetActive(false);
            return;
        }

        ShowLine();
    }

    void ShowLine()
    {
        StopAllCoroutines();

        textComponent.fontSize = 100;   // ensures it keeps the correct size

        textComponent.text = dialogueData[dialogueIndex].dialogue[lineIndex];
        textComponent.ForceMeshUpdate();
        textComponent.maxVisibleCharacters = 0;

        StartCoroutine(TypeText());
    }

    IEnumerator TypeText()
    {
        foreach (char c in textComponent.text)
        {
            textComponent.maxVisibleCharacters++;
            yield return new WaitForSeconds(charDelay);
        }
    }

    void Update()
    {
        if (dialogueBox.activeSelf && Input.GetKeyDown(KeyCode.E))
        {
            NextLine();
        }
    }
}