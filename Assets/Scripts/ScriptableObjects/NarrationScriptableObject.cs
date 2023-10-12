using Microsoft.MixedReality.Toolkit.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Narration", menuName = "Narration/NarrationSequence")]
public class NarrationScriptableObject : ScriptableObject
{
    public DialogueScriptableObject[] dialogues;

    public DialogueScriptableObject GetCurrentDialogue(int currentDialogueIndex)
    {
        return dialogues[currentDialogueIndex];
    }
}
