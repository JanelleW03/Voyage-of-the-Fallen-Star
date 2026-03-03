using UnityEngine;
using System.Collections.Generic;

public class NPC : MonoBehaviour
{
    public List<DialogueField> dialogueField;
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Player.interactionOccurence.AddListener(StartTalking);
        }
    }
    void StartTalking()
    {
        DialogueManager.NPCSpeaking.Invoke(dialogueField);
    }
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Player.interactionOccurence.RemoveListener(StartTalking);
        }
    }
}
