using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;

public class DialogueManager : MonoBehaviour
{
    public TextMeshProUGUI textComponent;
    public string[] lines;
    public float textSpeed;

    private InputAction _continueAction;
    private int _index;
    
    private void Start()
    {
        textComponent.text = string.Empty;
        _continueAction = InputSystem.actions.FindAction("Continue");
        StartDialogue();
    }

    private void Update()
    {
        if (_continueAction.WasPerformedThisFrame())
        {
            if(textComponent.text == lines[_index])
            {
                NextLine();
            }
            else
            {
                StopAllCoroutines();
                textComponent.text  = lines[_index];
            }
        }
    }
    private void StartDialogue()
    {
        _index = 0;
        StartCoroutine(TypeLine());
    }

    private IEnumerator TypeLine()
    {
        foreach (var c in lines[_index])
        {
            textComponent.text += c;
            yield return new WaitForSeconds(textSpeed);
        }
    }

    private void NextLine()
    {
        if(_index < lines.Length - 1)
        {
            _index++;
            textComponent.text = string.Empty;
            StartCoroutine(TypeLine());
        }
        else
        {
            gameObject.transform.parent.gameObject.SetActive(false);
        }
    }
}

