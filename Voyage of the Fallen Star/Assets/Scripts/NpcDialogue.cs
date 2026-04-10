using System.Collections.Generic;
using UnityEngine;

public class NpcDialogue : MonoBehaviour
{
    public List<DialogueField> dialogue;

    private DialogueManager _dialogueManager;
    private bool _playerInRange;
    private int _conversationIndex = 0;

    private void Start()
    {
        _dialogueManager = FindFirstObjectByType<DialogueManager>();
    }

    private void Update()
    {
        if (_playerInRange && Input.GetKeyDown(KeyCode.E) && !_dialogueManager.IsDialogueActive())
        {
            if (_conversationIndex < dialogue.Count)
            {
                _dialogueManager.StartDialogue(new List<DialogueField> { dialogue[_conversationIndex] });
                _conversationIndex++;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            _playerInRange = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            _playerInRange = false;
    }
}