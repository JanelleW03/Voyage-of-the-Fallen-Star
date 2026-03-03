using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
[RequireComponent(typeof(TMP_Text))]

public class DialogueManager : MonoBehaviour
{
    public static UnityEvent<List<DialogueField>> NPCSpeaking = new UnityEvent<List<DialogueField>>();
    public static UnityEvent TxtPrinted = new UnityEvent();
    List<DialogueField> txtRef;
    public GameObject txtParent;
    int dialogueOrder;
    int dialogueSet;
    TMP_Text txtComponent;
    // List<AudioClip>;
    float charDelay = 0.5f;

    void Awake()
    {
        NPCSpeaking.AddListener(DialogueCall);
        txtParent.SetActive(false);
    }

    void DialogueCall(List<DialogueField> localText)
    {
        if (!txtParent.activeInHierarchy)
        {
            txtParent.SetActive(true);
            txtComponent = GetComponent<TMP_Text>();
            dialogueOrder = 0;
            dialogueSet = 0;
            //put in the audio source here
            txtRef = localText;
            txtComponent.text = txtRef[dialogueSet].dialogue[dialogueOrder];
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
                if(dialogueOrder < txtRef[dialogueSet].dialogue.Count && gameObject.activeInHierarchy)
                {
                    txtComponent.text = txtRef[dialogueSet].dialogue[dialogueOrder];
                    StartCoroutine(WriteChar());
                }
                else if (dialogueOrder >= txtRef[dialogueSet].dialogue.Count)
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

