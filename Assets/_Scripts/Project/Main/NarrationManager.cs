using GoNatureAR.Requests;
using GoNatureAR.Sensors;
using Microsoft.MixedReality.Toolkit.Input;
using Microsoft.MixedReality.Toolkit.UI;
using System;
using System.Threading.Tasks;
using UnityEngine;
using Timer = System.Timers.Timer;
using static GoNatureAR.SpeechProfile;
using System.Timers;

namespace GoNatureAR
{
    [RequireComponent(typeof(SpeechInputHandler))]
    public class NarrationManager : MonoBehaviour
    {
        public static NarrationManager Instance { get; private set; }

        public static Action<DialogueScriptableObject> OnDialogueTrigger;
        public static Action OnIntroductionEnded;
        public static Action OnThermalSceneEnded;
        public static Action OnAirQualitySceneEnded;
        public static Action OnNoiseExposureSceneEnded;
        public static Action OnConfirmed;
        public static Action OnReset;

        [SerializeField]
        private NarrationScriptableObject _narrationSequence;
        [SerializeField]
        private GameObject _pilotsPanel;

        private SpeechInputHandler speechInputHandler;
        private DialogueKey currentDialogueKey;
        private Timer timer;
        private bool isNarratorReady;

        private void Awake()
        {
            if (Instance == null)
                Instance = this;
            else
                Destroy(this);
        }

        private void OnEnable()
        {
            isNarratorReady = true;
            timer = new Timer(20000f);
            timer.Elapsed += OnThresholdPassed;

            GameManager.OnChangeState += OnChangeStateHandler; 
            PilotDataRequestManager.OnDataReceived += () => GetDialogue(Keyword.Done);

            ThermalComfortManager.OnInfoButtonPressed += InfoDialogueHandle;
            ThermalComfortManager.OnThermalComfortCalculated += OnThermalComfortCalcualtedHandle;

            AirQualityManager.OnAirQualityCalculated += OnAirQualityCalculatedHandle;
            AirQualityManager.OnInfoButtonPressed += OnButtonPressedHandle;

            NoiseExposureManager.OnNoiseCalculated += OnNoiseCalculatedHandle;
            NoiseExposureManager.OnHandMenuEnabled += OnMenuEnabledHandle;

            OutroManager.OnAirButtonPressed += () => GetDialogue(Keyword.Comfortable);
            OutroManager.OnNoiseButtonPressed += () => GetDialogue(Keyword.Loud);
            OutroManager.OnHeatButtonPressed += () => GetDialogue(Keyword.Hot);
            OutroManager.OnOutroEnded += () => OnReset?.Invoke();
        }

        private void OnDisable()
        {
            GameManager.OnChangeState -= OnChangeStateHandler;

            ThermalComfortManager.OnInfoButtonPressed -= InfoDialogueHandle;
            ThermalComfortManager.OnThermalComfortCalculated -= OnThermalComfortCalcualtedHandle;

            AirQualityManager.OnAirQualityCalculated -= OnAirQualityCalculatedHandle;
            AirQualityManager.OnInfoButtonPressed -= OnButtonPressedHandle;

            NoiseExposureManager.OnNoiseCalculated -= OnNoiseCalculatedHandle;
            NoiseExposureManager.OnHandMenuEnabled -= OnMenuEnabledHandle;
        }

        private void Start()
        {
            speechInputHandler = GetComponent<SpeechInputHandler>();

            speechInputHandler.AddResponse(GetKeywordToString(Keyword.Julie), JulieCalled);
            speechInputHandler.AddResponse(GetKeywordToString(Keyword.Continue), () => GetDialogue(Keyword.Continue));
            speechInputHandler.AddResponse(GetKeywordToString(Keyword.Yes), () => GetDialogue(Keyword.Yes));
            speechInputHandler.AddResponse(GetKeywordToString(Keyword.LetsGo), () => GetDialogue(Keyword.LetsGo));
            speechInputHandler.AddResponse("Restart", () => OnReset?.Invoke());
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
                    GetDialogue(Keyword.Intro);
                    break;
            }
        }

        private async void GetDialogue(Keyword keyword)
        {   
            //Before talk
            if (keyword == Keyword.Yes && (currentDialogueKey.State == State.Temperature ||
                                          currentDialogueKey.State == State.Noise ||
                                          currentDialogueKey.State == State.AirQuality))
            {
                OnConfirmed?.Invoke();
                return;
            }

            if (keyword == Keyword.LetsGo && currentDialogueKey.State == State.Temperature)
            {
                if (!isNarratorReady)
                    return;

                isNarratorReady = false;
                timer.Enabled = true;

                await Task.Delay(2000);
                OnThermalSceneEnded?.Invoke();
                return;
            }

            if (keyword == Keyword.Continue && currentDialogueKey.State == State.AirQuality)
            {
                if (!isNarratorReady)
                    return;

                isNarratorReady = false;
                timer.Enabled = true;

                await Task.Delay(2000);
                OnAirQualitySceneEnded?.Invoke();
                return;
            }

            //Talk
            currentDialogueKey.Keyword = keyword;
            Debug.Log(currentDialogueKey.State + " " + currentDialogueKey.Keyword );
            OnDialogueTrigger?.Invoke(_narrationSequence.GetDialogueByKey(currentDialogueKey));

            //After talk
            if (currentDialogueKey.State == State.Introduction && currentDialogueKey.Keyword == Keyword.Continue)
            {
                if (!isNarratorReady)
                    return;

                isNarratorReady = false;
                timer.Enabled = true;

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

        #region AIR QUALITY
        private void OnAirQualityCalculatedHandle(AirIndex index)
        {
            switch (index)
            {
                case AirIndex.Good:
                    GetDialogue(Keyword.Comfortable);
                    break;
                case AirIndex.Unhealthy:
                    GetDialogue(Keyword.Loud);
                    break;
            }
        }

        private async void OnButtonPressedHandle(AirIndex index)
        {
            switch (index)
            {
                case AirIndex.Good:
                    GetDialogue(Keyword.Done);
                    break;
                case AirIndex.Unhealthy:
                    GetDialogue(Keyword.Info);
                    await Task.Delay(15000);
                    GetDialogue(Keyword.Select);
                    break;
            }
        }
        #endregion

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

        private async void InfoDialogueHandle(ThermalComofortIndex index)
        {
            if(currentDialogueKey.State == State.Temperature)
            {
                if (index == ThermalComofortIndex.Cold || index == ThermalComofortIndex.Comfortable)
                    GetDialogue(Keyword.Done);
                else
                {
                    GetDialogue(Keyword.Info); 
                    await Task.Delay(25000);
                    GetDialogue(Keyword.Select);
                }
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

        private async void OnMenuEnabledHandle(NoiseExposureIndex index)
        {
            switch (index)
            {
                case NoiseExposureIndex.Faint:
                    GetDialogue(Keyword.Done);
                    break;
                case NoiseExposureIndex.Loud:
                    GetDialogue(Keyword.Info);
                    await Task.Delay(20000);
                    OnNoiseExposureSceneEnded?.Invoke();
                    break;
            }
        }
        #endregion

        private void OnThresholdPassed(object sender, ElapsedEventArgs e)
        {
            isNarratorReady = true;
            timer.Enabled = false;
        }
    }
}
