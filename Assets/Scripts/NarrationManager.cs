using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class NarrationManager : MonoBehaviour
{
    [SerializeField]
    private NarrationScriptableObject narrationSequence;

    [SerializeField]
    private TextAnim textAnimator;

    [SerializeField]
    private Button button;

    UnityEvent dialogueTrigger = new UnityEvent();

    private int currentDialogueIndex;
    private AudioSource audioSource;

    private void Awake() 
    {
        dialogueTrigger.AddListener(TriggerNextDialogue);
    }

    private void Start()
    {
        currentDialogueIndex = 0;
        audioSource = GetComponent<AudioSource>();
        button.onClick.AddListener(() => dialogueTrigger?.Invoke());
    }

    public void TriggerNextDialogue()
    {
        if (currentDialogueIndex >= narrationSequence.dialogues.Length) 
        {
            currentDialogueIndex= 0;
            return;    
        }
        audioSource.Stop();

        var  dialogue = narrationSequence.GetCurrentDialogue(currentDialogueIndex);
        string narrationText = dialogue.text;
        AudioClip audioClip = dialogue.audioClip;

        audioSource.PlayOneShot(audioClip);
        textAnimator.TypeText(narrationText);
        Debug.Log(narrationText);

        currentDialogueIndex++;
    }
}
