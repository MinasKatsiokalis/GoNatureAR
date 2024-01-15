using UnityEngine;

namespace GoNatureAR
{
    [CreateAssetMenu(fileName = "Narration", menuName = "Narration/NarrationSequence")]
    public class NarrationScriptableObject : ScriptableObject
    {
        public DialogueScriptableObject[] dialogues;

        public DialogueScriptableObject GetDialogueByKey(DialogueKey key)
        {
            foreach (var dialogue in dialogues)
            {
                if (dialogue.DialogueKey.Equals(key))
                    return dialogue;
            }
            return null;
        }
    }
}
