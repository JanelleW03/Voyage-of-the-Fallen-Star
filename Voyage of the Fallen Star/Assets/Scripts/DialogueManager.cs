using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class DialogueManager : MonoBehaviour
{

    public AudioSource dialogueAudioSource; 
    public GameObject dialogueBox;
    public TMP_Text textComponent;

    private List<DialogueField> _dialogueData;
    private int _dialogueIndex;
    private int _lineIndex;
    private float _charDelay = 0.02f;

    private PlayerMovementController _playerMovement;

    private InputAction _interactAction;

    public TMP_Text nameComponent;   // drag "Character Name" here

    private void Awake()
    {
        if (textComponent == null)
        {
            Debug.LogError("TMP_Text not found inside DialogueManager!");
        }
    }

    public bool IsDialogueActive()
    {
        return dialogueBox.activeSelf;
    }

    private void Start()
    {
        dialogueBox.SetActive(false);
        _interactAction = InputSystem.actions.FindAction("Interact");
        _playerMovement = FindFirstObjectByType<PlayerMovementController>();
    }
    
    private void Update()
    {
        if (dialogueBox.activeSelf && Input.GetKeyDown(KeyCode.Space))
        {
            NextLine();
        }
    }
    public void StartDialogue(List<DialogueField> newDialogue)
    {
        _playerMovement?.SetMovementEnabled(false);
        _dialogueData = newDialogue;
        _dialogueIndex = 0;
        _lineIndex = 0;
        dialogueBox.SetActive(true);
        ShowLine();
    }

    private IEnumerator DelayedStart()
    {
        yield return new WaitForSeconds(3f);
        dialogueBox.SetActive(true);
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
            dialogueBox.SetActive(false);  // hides the box when dialogue ends
            _playerMovement?.SetMovementEnabled(true);
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

        if (nameComponent != null)
            nameComponent.text = _dialogueData[_dialogueIndex].speakingName[_lineIndex];

        // Play audio for this line
        var audioClips = _dialogueData[_dialogueIndex].dialogueAudio;
        if (dialogueAudioSource != null && _lineIndex < audioClips.Count && audioClips[_lineIndex] != null)
        {
            dialogueAudioSource.clip = audioClips[_lineIndex];
            dialogueAudioSource.Play();
        }

        StartCoroutine(TypeText());
    }
    private IEnumerator TypeText()
    {
        foreach (char _ in textComponent.text)
        {
            textComponent.maxVisibleCharacters++;
            yield return new WaitForSeconds(_charDelay);
        }
        dialogueAudioSource?.Stop();
    }
}