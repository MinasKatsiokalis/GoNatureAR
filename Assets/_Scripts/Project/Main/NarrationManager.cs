using GoNatureAR.Requests;
using GoNatureAR.Sensors;
using Microsoft.MixedReality.Toolkit.Input;
using Microsoft.MixedReality.Toolkit.UI;
using System;
using System.Threading.Tasks;
using UnityEngine;
using static GoNatureAR.SpeechProfile;

namespace GoNatureAR
{
    [RequireComponent(typeof(SpeechInputHandler))]
    public class NarrationManager : MonoBehaviour
    {
        public static NarrationManager Instance { get; private set; }

        public static Action OnIntroductionEnded;
        public static Action OnThermalSceneEnded;
        public static Action<DialogueScriptableObject> OnDialogueTrigger;
        public static Action OnConfirmed;

        [SerializeField]
        private NarrationScriptableObject _narrationSequence;
        [SerializeField]
        private GameObject _pilotsPanel;

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
            PilotDataRequestManager.OnDataReceived += () => GetDialogue(Keyword.Done);

            ThermalComfortManager.OnInfoButtonPressed += InfoDialogueHandle;
            ThermalComfortManager.OnThermalComfortCalculated += OnThermalComfortCalcualtedHandle;

            NoiseExposureManager.OnNoiseCalculated += OnNoiseCalculatedHandle;
        }

        private void OnDisable()
        {
            GameManager.OnChangeState -= OnChangeStateHandler;

            ThermalComfortManager.OnInfoButtonPressed -= InfoDialogueHandle;
            ThermalComfortManager.OnThermalComfortCalculated -= OnThermalComfortCalcualtedHandle;

            NoiseExposureManager.OnNoiseCalculated -= OnNoiseCalculatedHandle;
        }

        private void Start()
        {
            speechInputHandler = GetComponent<SpeechInputHandler>();

            speechInputHandler.AddResponse(GetKeywordToString(Keyword.Julie), JulieCalled);
            speechInputHandler.AddResponse(GetKeywordToString(Keyword.Continue), () => GetDialogue(Keyword.Continue));
            speechInputHandler.AddResponse(GetKeywordToString(Keyword.Yes), () => GetDialogue(Keyword.Yes));
            speechInputHandler.AddResponse(GetKeywordToString(Keyword.LetsGo), () => GetDialogue(Keyword.LetsGo));

        }

        private void OnChangeStateHandler(State state)
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

        private async void GetDialogue(Keyword keyword)
        {   
            //Before talk
            if(keyword == Keyword.Yes && currentDialogueKey.State == State.Temperature ||
               keyword == Keyword.Yes && currentDialogueKey.State == State.Noise)
            {
                OnConfirmed?.Invoke();
                return;
            }

            if (keyword == Keyword.Continue && currentDialogueKey.State == State.Temperature)
            {
                await Task.Delay(4000);
                OnThermalSceneEnded?.Invoke();
                return;
            }

            //Talk
            currentDialogueKey.Keyword = keyword;
            OnDialogueTrigger?.Invoke(_narrationSequence.GetDialogueByKey(currentDialogueKey));

            //After talk
            if (currentDialogueKey.State == State.Introduction && currentDialogueKey.Keyword == Keyword.Continue)
            {
                _pilotsPanel.SetActive(false);
                await Task.Delay(6000);
                OnIntroductionEnded?.Invoke();
                return;
            }

            if (currentDialogueKey.State == State.Introduction && currentDialogueKey.Keyword == Keyword.LetsGo)
            {
                _pilotsPanel.SetActive(true);
                await Task.Delay(2000);
                GetDialogue(Keyword.Select);
                return;
            }

        }
        private void JulieCalled()
        {
            OnDialogueTrigger?.Invoke(_narrationSequence.GetDialogueByKey(new DialogueKey(State.Introduction, Keyword.Julie)));
        }

        public void GetPalmNotUpDialogue()
        {
            OnDialogueTrigger?.Invoke(_narrationSequence.GetDialogueByKey(new DialogueKey(State.Introduction, Keyword.Palm)));
        }

        #region THERMAL
        private void OnThermalComfortCalcualtedHandle(ThermalComofortIndex index, double humidity)
        {
            switch (index)
            {
                case ThermalComofortIndex.Cold:
                    GetDialogue(Keyword.Cold);
                    break;
                case ThermalComofortIndex.Hot:
                    GetDialogue(Keyword.Hot);
                    break;
                default:
                    GetDialogue(Keyword.Comfortable);
                    break;
            }
        }

        private void InfoDialogueHandle(ThermalComofortIndex index)
        {
            if(currentDialogueKey.State == State.Temperature)
            {
                if (index == ThermalComofortIndex.Cold || index == ThermalComofortIndex.Comfortable)
                    GetDialogue(Keyword.Done);
                else
                    GetDialogue(Keyword.Info);
            }
        }
        #endregion

        #region NOISE
        private void OnNoiseCalculatedHandle(NoiseExposureIndex index)
        {
            switch (index)
            {
                case NoiseExposureIndex.Faint:
                    GetDialogue(Keyword.Comfortable);
                    break;
                case NoiseExposureIndex.Loud:
                    GetDialogue(Keyword.Loud);
                    break;
            }
        }
        #endregion
    }
}
