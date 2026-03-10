using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class NpcDialogue : MonoBehaviour
{
    public List<DialogueField> dialogue;
    
    private DialogueManager _dialogueManager;
    private InputAction _interactAction;

    private bool _playerInRange;

    private void Start()
    {
        _dialogueManager = FindFirstObjectByType<DialogueManager>();
        if (_dialogueManager == null)
        {
            Debug.LogError("DialogueManager not found in scene!");
        }
        
        _interactAction = InputSystem.actions.FindAction("Interact");
    }

    private void Update()
    {
        if (_playerInRange && _interactAction.WasPerformedThisFrame())
        {
            _dialogueManager.StartDialogue(dialogue);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _playerInRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _playerInRange = false;
        }
    }
}