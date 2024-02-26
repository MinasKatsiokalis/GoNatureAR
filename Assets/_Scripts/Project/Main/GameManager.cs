using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GoNatureAR
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }

        public static event Action<State> OnChangeState;

        private State state;
        public State CurrentState
        {
            get => state;
        }

        private int currentStateIndex;

        void Awake()
        {
            if (Instance == null)
                Instance = this;
            else
                Destroy(this);
        }

        private void OnEnable()
        {
            NarrationManager.OnIntroductionEnded += GoToNextState;
            NarrationManager.OnThermalSceneEnded += GoToNextState;
            NarrationManager.OnAirQualitySceneEnded += GoToNextState;
            NarrationManager.OnNoiseExposureSceneEnded += GoToNextState;
            NarrationManager.OnReset += Restart;
        }

        private void OnDisable()
        {
            NarrationManager.OnIntroductionEnded -= GoToNextState;
            NarrationManager.OnThermalSceneEnded -= GoToNextState;
            NarrationManager.OnAirQualitySceneEnded -= GoToNextState;
            NarrationManager.OnNoiseExposureSceneEnded -= GoToNextState;
            NarrationManager.OnReset -= Restart;
        }

        // Start is called before the first frame update
        void Start()
        {
            //First state == 0
            currentStateIndex = 0;
            SetCurrentState(State.Introduction);
        }

        private void GoToNextState()
        {
            currentStateIndex++;

            State[] states = (State[])Enum.GetValues(typeof(State));
            if (currentStateIndex >= states.Length)
                return;

            // Set the current state to the next state
            SetCurrentState(states[currentStateIndex]);
        }

        private void SetCurrentState(State state)
        {
            switch (state)
            {
                case State.Introduction:
                    this.state = state;
                    OnChangeState?.Invoke(this.state);
                    break;
                case State.AirQuality:
                    this.state = state;
                    SceneManager.UnloadSceneAsync("ThermalComfort");
                    SceneManager.LoadSceneAsync("AirQuality", LoadSceneMode.Additive);
                    SceneManager.sceneLoaded += (scene, mode) => OnChangeState?.Invoke(this.state);
                    break;
                case State.Noise:
                    this.state = state;
                    SceneManager.UnloadSceneAsync("AirQuality");
                    SceneManager.LoadSceneAsync("NoiseExposure", LoadSceneMode.Additive);
                    SceneManager.sceneLoaded += (scene, mode) => OnChangeState?.Invoke(this.state);
                    break;
                case State.Temperature:
                    this.state = state;
                    SceneManager.LoadSceneAsync("ThermalComfort", LoadSceneMode.Additive);
                    SceneManager.sceneLoaded += (scene, mode) => OnChangeState?.Invoke(this.state);
                    break;
                case State.Outro:
                    this.state = state;
                    SceneManager.UnloadSceneAsync("NoiseExposure");
                    SceneManager.LoadSceneAsync("Outro", LoadSceneMode.Additive);
                    SceneManager.sceneLoaded += (scene, mode) => OnChangeState?.Invoke(this.state);
                    break;
                default:
                    break;
            }
        }

        private void Restart()
        {   
            // Start from the end to avoid index changes due to scene unloading
            for (int i = SceneManager.sceneCount - 1; i > 0; i--) 
            {
                Scene scene = SceneManager.GetSceneAt(i);
                if (scene.isLoaded)
                {
                    SceneManager.UnloadSceneAsync(scene);
                }
            }
            SceneManager.LoadScene(0);
        }
    }


    [Serializable]
    public enum State
    {
        Introduction,
        Temperature,
        AirQuality,
        Noise,
        Outro
    }
}