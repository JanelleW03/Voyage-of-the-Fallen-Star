using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcDialogueFight : MonoBehaviour
{
    [Header("Dialogue")]
    public DialogueField dialogue;


    [Header("Combat")]
    public EnemyController enemyController;
    public NpcEnemyHealthComponent healthComponent;

    [Header("Room")]
    public DoorTrigger exitDoor;

    private DialogueManager _dialogueManager;
    private bool _dialoguePlayed = false;

    private void Start()
    {
        _dialogueManager = FindFirstObjectByType<DialogueManager>();

        if (enemyController != null)
            enemyController.isHostile = false;

        if (healthComponent != null && healthComponent.healthSlider != null)
            healthComponent.healthSlider.gameObject.SetActive(false);

        // Lock the door at the start
        if (exitDoor != null)
            exitDoor.SetLocked(true);

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !_dialoguePlayed)
        {
            _dialoguePlayed = true;
            StartCoroutine(RunDialogueAndFight());
        }
    }

    private IEnumerator RunDialogueAndFight()
    {
        // Wait if another dialogue is already playing
        yield return new WaitUntil(() => !_dialogueManager.IsDialogueActive());

        _dialogueManager.StartDialogue(new List<DialogueField> { dialogue });

        // Wait one frame so IsDialogueActive() becomes true
        yield return null;

        // Wait for dialogue to finish
        yield return new WaitUntil(() => !_dialogueManager.IsDialogueActive());

        // Start combat
        healthComponent?.StartCombat();
        if (enemyController != null)
            enemyController.SetHostile(true);

    }

    public IEnumerator OnNpcDefeated()
    {
        Debug.Log("OnNpcDefeated started");
        yield return new WaitUntil(() => !_dialogueManager.IsDialogueActive());
        Debug.Log("Dialogue clear, starting defeat dialogue");

        _dialogueManager.StartDialogue(new List<DialogueField> { dialogue });
        yield return null;
        yield return new WaitUntil(() => !_dialogueManager.IsDialogueActive());
        Debug.Log("Defeat dialogue finished");

        if (exitDoor != null)
        {
            Debug.Log("Unlocking door");
            exitDoor.SetLocked(false);
        }
        else
        {
            Debug.Log("exitDoor is NULL!");
        }
    }



}