using NUnit.Framework;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
[RequireComponent(typeof(TMP_Text))]

public class DialogueManager : MonoBehaviour
{
    public static UnityEvent<DialogueField> NPCSpeaking = new UnityEvent<DialogueField>();
    public static UnityEvent TxtPrinted = new UnityEvent();
    DialogueField txtRef;
    public GameObject txtParent;
    int dialogueOrder;
    TMP_Text txtComponent;
    // List<AudioClip>;
    float charDelay = 0.5f;

    void Awake()
    {
        NPCSpeaking.AddListener(DialogueCall);
    }

    void DialogueCall(DialogueField localText)
    {
        if (!txtParent.activeInHierarchy)
        {
            txtParent.SetActive(true);
            txtComponent = GetComponent<TMP_Text>();
            dialogueOrder = 0;
            //put in the audio source here
            txtRef = localText;
            txtComponent.text = txtRef.dialogue[dialogueOrder];
            StartCoroutine(WriteChar());
        } else
        {
            StopAllCoroutines();
            if (txtComponent.maxVisibleCharacters < txtComponent.text.Length)
            {
                txtComponent.maxVisibleCharacters = txtComponent.text.Length;
            }
            else
            {
                dialogueOrder++;
                //advance the audio dialogue here 
                if(dialogueOrder < txtRef.dialogue.Count && gameObject.activeInHierarchy)
                {
                    txtComponent.text = txtRef.dialogue[dialogueOrder];
                    StartCoroutine(WriteChar());
                }
                else if (dialogueOrder >= txtRef.dialogue.Count)
                {
                    txtParent.SetActive(false);
                }
            }
        }
    }

    IEnumerator WriteChar()
    {
        txtComponent.maxVisibleCharacters = 0;
        foreach (char c in txtComponent.text)
        {
            txtComponent.maxVisibleCharacters++;
            yield return new WaitForSeconds(charDelay);
        }
        txtComponent.maxVisibleCharacters = 99999;
        TxtPrinted.Invoke();
    }
}

