using Microsoft.MixedReality.Toolkit.UI;
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
        audioSource = GetComponent<AudioSource>();
        button.onClick.AddListener(() => dialogueTrigger?.Invoke());
    }

    public void TriggerNextDialogue()
    {
        string narrationText;
        AudioClip audioClip;

        if(!narrationSequence.GetCurrentDialogue(out narrationText, out audioClip))
            return;

        audioSource.PlayOneShot(audioClip);
        textAnimator.TypeText(narrationText);
        Debug.Log(narrationText);
    }
}
