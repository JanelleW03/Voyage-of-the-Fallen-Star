using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DialogueField", menuName = "Scriptable Object")]
public class DialogueField : ScriptableObject
{
    public List<Sprite> portraits;
    public List<string> dialogue;
    public List<string> speakingName;
    public List<AudioClip> dialogueAudio;
}
