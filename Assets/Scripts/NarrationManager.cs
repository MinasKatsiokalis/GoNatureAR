using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class NarrationManager : MonoBehaviour
{
    public static NarrationManager Instance { get; private set; }

    public static Action dialogueTrigger;

    [SerializeField]
    private NarrationScriptableObject narrationSequence;
    private int currentDialogueIndex;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            //DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnEnable() 
    {
        //dialogueTrigger += TriggerNextDialogue;
    }

    private void OnDisable()
    {
        //dialogueTrigger -= TriggerNextDialogue;
    }

    private void Start()
    {
        currentDialogueIndex = 0;
    }

    public DialogueScriptableObject TriggerNextDialogue()
    {
        if (currentDialogueIndex >= narrationSequence.dialogues.Length) 
        {
            currentDialogueIndex= 0;
            return null;
        }
        currentDialogueIndex++;
        return narrationSequence.GetCurrentDialogue(currentDialogueIndex);
    }
}
