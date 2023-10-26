using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.UI;

public class NarrationManager : MonoBehaviour
{
    public static NarrationManager Instance { get; private set; }

    //public static event Action<DialogueScriptableObject> OnDialogueTrigger;

    [SerializeField]
    private NarrationScriptableObject narrationSequence;
    private DialogueKey currentDialogueKey;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this);
    }

    private void OnEnable() 
    {
        GameManager.OnChangeState += OnChangeStateHandler;
    }

    private void OnDisable()
    {
        GameManager.OnChangeState -= OnChangeStateHandler;
    }

    void OnChangeStateHandler(State state)
    {
        switch (state)
        {
            case State.Introduction:
                currentDialogueKey.State = State.Introduction;
                break;
            case State.AirQuality:
                currentDialogueKey.State = State.AirQuality;
                break;
            case State.Noise:
                currentDialogueKey.State = State.Noise;
                break;
            case State.Temperature:
                currentDialogueKey.State = State.Temperature;
                break;
            case State.Outro:
                currentDialogueKey.State = State.Outro;
                break;
        }
        currentDialogueKey.Keyword = "FirstDialogue";
    }

    DialogueScriptableObject GetDialogue()
    {
        DialogueScriptableObject dialogue = narrationSequence.GetDialogueByKey(currentDialogueKey);
        return dialogue;
    }

}
