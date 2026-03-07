using UnityEngine;
using System.Collections.Generic;

public class NPC : MonoBehaviour
{
    public List<DialogueField> dialogueField;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Entered NPC trigger");
            PlayerController.interactionOccurence.AddListener(StartTalking);
        }
    }

    void StartTalking()
    {
        Debug.Log("Talking triggered!");
        DialogueManager.NPCSpeaking.Invoke(dialogueField);
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerController.interactionOccurence.RemoveListener(StartTalking);
        }
    }
}