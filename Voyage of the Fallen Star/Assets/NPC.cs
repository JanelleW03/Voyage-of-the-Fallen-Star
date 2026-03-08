using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour
{
    public List<DialogueField> dialogue;
    DialogueManager dialogueManager;

    bool playerInRange = false;

    void Start()
    {
        dialogueManager = FindFirstObjectByType<DialogueManager>();
        if (dialogueManager == null)
            Debug.LogError("DialogueManager not found in scene!");
    
    }

    void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            dialogueManager.StartDialogue(dialogue);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            playerInRange = true;
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            playerInRange = false;
    }
}