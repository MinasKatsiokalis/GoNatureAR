using Microsoft.MixedReality.Toolkit.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Narration", menuName = "Narration/NarrationSequence")]
public class NarrationScriptableObject : ScriptableObject
{
    public DialogueScriptableObject[] dialogues;
    private int currentDialogueIndex = 0;

    public bool GetCurrentDialogue(out string text, out AudioClip audio)
    {
        if (currentDialogueIndex < dialogues.Length)
        {
            audio = dialogues[currentDialogueIndex].audioClip;
            text = dialogues[currentDialogueIndex].text;
            
            currentDialogueIndex++;
            return true;
        }
        Reset();
        text = null;
        audio = null;

        return false;
    }

    private void Reset()
    {
        currentDialogueIndex = 0;
    }
}
