using Microsoft.MixedReality.Toolkit.Input;
using System;
using System.Collections.Generic;
using UnityEngine;
using static SpeechProfile;

[RequireComponent(typeof(SpeechInputHandler))]
public class NarrationManager : MonoBehaviour
{
    public static NarrationManager Instance { get; private set; }

    public static event Action<DialogueScriptableObject> OnDialogueTrigger;

    [SerializeField]
    private NarrationScriptableObject _narrationSequence;

    private SpeechInputHandler speechInputHandler;
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

    private void Start()
    {
        speechInputHandler = GetComponent<SpeechInputHandler>();
        speechInputHandler.AddResponse(GetKeywordToString(Keyword.Julie), () => GetDialogue(Keyword.Julie));
        speechInputHandler.AddResponse(GetKeywordToString(Keyword.Continue), () => GetDialogue(Keyword.Continue));
        speechInputHandler.AddResponse(GetKeywordToString(Keyword.Yes), () => GetDialogue(Keyword.Yes));
        speechInputHandler.AddResponse(GetKeywordToString(Keyword.LetsGo), () => GetDialogue(Keyword.LetsGo));
    }

    void OnChangeStateHandler(State state)
    {
        switch (state)
        {
            case State.Introduction:
                currentDialogueKey.State = State.Introduction;
                GetDialogue(Keyword.Intro);
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
    }

    void GetDialogue(Keyword keyword)
    {
        currentDialogueKey.Keyword = keyword;
        OnDialogueTrigger?.Invoke(_narrationSequence.GetDialogueByKey(currentDialogueKey));
    }

    public void GetPalmNotUpDialogue()
    {
        OnDialogueTrigger?.Invoke(_narrationSequence.GetDialogueByKey(new DialogueKey(State.Introduction, Keyword.Palm)));
    }
}
