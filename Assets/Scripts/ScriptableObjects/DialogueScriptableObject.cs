using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Dialogue", menuName = "Narration/Dialogue")]
public class DialogueScriptableObject : ScriptableObject
{
    [TextArea(3, 10)]
    public string text;
    public AudioClip audioClip;
}