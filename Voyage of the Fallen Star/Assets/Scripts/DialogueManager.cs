using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class DialogueManager : MonoBehaviour
{
    public GameObject dialogueBox;
    public TMP_Text textComponent;

    private List<DialogueField> _dialogueData;
    private int _dialogueIndex;
    private int _lineIndex;
    private float _charDelay = 0.02f;
    
    private InputAction _interactAction;

    private void Awake()
    {
        if (textComponent == null)
        {
            Debug.LogError("TMP_Text not found inside DialogueManager!");
        }
    }

    private void Start()
    {
        dialogueBox.SetActive(false);
        _interactAction = InputSystem.actions.FindAction("Interact");
    }
    
    private void Update()
    {
        if (dialogueBox.activeSelf && _interactAction.WasPerformedThisFrame())
        {
            NextLine();
        }
    }

    public void StartDialogue(List<DialogueField> newDialogue)
    {
        dialogueBox.SetActive(true);
        _dialogueData = newDialogue;
        _dialogueIndex = 0;
        _lineIndex = 0;
        ShowLine();
    }

    private void NextLine()
    {
        if (textComponent.maxVisibleCharacters < textComponent.text.Length)
        {
            textComponent.maxVisibleCharacters = textComponent.text.Length;
            return;
        }

        _lineIndex++;

        if (_lineIndex >= _dialogueData[_dialogueIndex].dialogue.Count)
        {
            dialogueBox.SetActive(false);
            return;
        }

        ShowLine();
    }

    private void ShowLine()
    {
        StopAllCoroutines();
        textComponent.fontSize = 100; 
        textComponent.text = _dialogueData[_dialogueIndex].dialogue[_lineIndex];
        textComponent.ForceMeshUpdate();
        textComponent.maxVisibleCharacters = 0;

        StartCoroutine(TypeText());
    }

    private IEnumerator TypeText()
    {
        foreach (char _ in textComponent.text)
        {
            textComponent.maxVisibleCharacters++;
            yield return new WaitForSeconds(_charDelay);
        }
    }
}