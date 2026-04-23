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

    private DialogueManager _dialogueManager;
    private bool _dialoguePlayed = false;

    private void Start()
    {
        _dialogueManager = FindFirstObjectByType<DialogueManager>();

        if (enemyController != null)
            enemyController.isHostile = false;

        if (healthComponent != null && healthComponent.healthSlider != null)
            healthComponent.healthSlider.gameObject.SetActive(false);
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
            enemyController.isHostile = true;
    }
}