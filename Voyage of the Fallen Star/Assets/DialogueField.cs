using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DialogueField", menuName = "Scriptable Object")]
public class DialogueField : ScriptableObject
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public List<Sprite> potraits;
    public List<string> dialogue;
    public List<string> speakingName;
    public List<AudioClip> dialogueAudio;
}
