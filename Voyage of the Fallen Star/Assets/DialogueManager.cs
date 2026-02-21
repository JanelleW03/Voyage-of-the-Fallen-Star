using NUnit.Framework;
using UnityEngine;
using UnityEngine.Events;

public class DialogueManager : MonoBehaviour
{
    public static UnityEvent<DialogueField> NPCSpeaking = new UnityEvent<DialogueField>();
    DialogueField TextRef;
    // List<AudioClip>;
    
    private void Start()
    {
       
    }

    private void Update()
    {
       
    }
    
}

