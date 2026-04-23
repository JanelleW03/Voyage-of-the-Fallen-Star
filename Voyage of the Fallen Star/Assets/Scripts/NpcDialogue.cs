using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcDialogue : MonoBehaviour
{
    [Header("Dialogue Assets")]
    public DialogueField startDialogue;
    public DialogueField afterLetterDialogue;

    private DialogueManager _dialogueManager;
    private Inventory _inventory;
    private EnemyController _enemyController;
    private NpcEnemyHealthComponent _healthComponent;

    private void Start()
    {
        _dialogueManager = FindFirstObjectByType<DialogueManager>();
        _inventory = FindFirstObjectByType<Inventory>();
        _enemyController = GetComponent<EnemyController>();
        _healthComponent = GetComponent<NpcEnemyHealthComponent>();

        _enemyController.isHostile = false;

        if (_healthComponent.healthSlider != null)
            _healthComponent.healthSlider.gameObject.SetActive(false);

        StartCoroutine(RunDialogueSequence());
    }

    private IEnumerator RunDialogueSequence()
    {
        yield return null;

        _dialogueManager.StartDialogue(new List<DialogueField> { startDialogue });
        yield return new WaitUntil(() => !_dialogueManager.IsDialogueActive());

        yield return new WaitUntil(() => _inventory.HasLetter());
        yield return new WaitUntil(() => !_inventory.IsInventoryClosed());
        yield return new WaitUntil(() => _inventory.IsInventoryClosed());

        yield return null;

        _dialogueManager.StartDialogue(new List<DialogueField> { afterLetterDialogue });
        yield return new WaitUntil(() => !_dialogueManager.IsDialogueActive());

        _healthComponent.StartCombat();
        _enemyController.isHostile = true;
    }
}