using DG.Tweening;
using Microsoft.MixedReality.Toolkit.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GoNatureAR
{
    public class OutroManager : MonoBehaviour
    {   
        public static Action OnAirButtonPressed;
        public static Action OnNoiseButtonPressed;
        public static Action OnHeatButtonPressed;
        public static Action OnCreditsEnabled;
        public static Action OnOutroEnded;

        [SerializeField]
        AudioClip _outroAmbientMusic;
        [SerializeField]
        GameObject _environment;
        [SerializeField]
        GameObject _buttons;

        [SerializeField]
        private Interactable _airButton;
        [SerializeField]
        private Interactable _noiseButton;
        [SerializeField]
        private Interactable _heatButton;

        private int activeButtonsCount = 3;

        private Camera mainCamera;
        private void OnEnable()
        {
            mainCamera = Camera.main;
            GameManager.OnChangeState += OnChangeStateHandler;
        }

        private void OnDisable()
        {
            GameManager.OnChangeState -= OnChangeStateHandler;
        }
        // Start is called before the first frame update
        void Start()
        {
            _airButton.OnClick.AddListener(OnAirButtonPressedHandle);
            _noiseButton.OnClick.AddListener(OnNoiseButtonPressedHandle);
            _heatButton.OnClick.AddListener(OnHeatButtonPressedHandle);
        }

        private void OnAirButtonPressedHandle()
        {
            _airButton.gameObject.SetActive(false);
            OnAirButtonPressed?.Invoke();
            activeButtonsCount--;

            if(activeButtonsCount <= 0)
                OnCreditsEnabled?.Invoke();
        }

        private void OnNoiseButtonPressedHandle()
        {
            _noiseButton.gameObject.SetActive(false);
            OnNoiseButtonPressed?.Invoke();
            activeButtonsCount--;

            if (activeButtonsCount <= 0)
                OnCreditsEnabled?.Invoke();
        }

        private void OnHeatButtonPressedHandle()
        {
            _heatButton.gameObject.SetActive(false);
            OnHeatButtonPressed?.Invoke();
            activeButtonsCount--;

            if (activeButtonsCount <= 0)
                OnCreditsEnabled?.Invoke();
        }

        private void OnChangeStateHandler(State state)
        {
            if (state == State.Outro)
            {
                MusicManager.Instance.PlayMusic(_outroAmbientMusic); 
                _environment.transform.position = mainCamera.transform.position + mainCamera.transform.forward * 2f;
                _environment.transform.position = new Vector3(_environment.transform.position.x, mainCamera.transform.position.y - 1.4f, _environment.transform.position.z);
                _environment.transform.LookAt(mainCamera.transform);
                _environment.transform.rotation = Quaternion.Euler(0, _environment.transform.rotation.eulerAngles.y, 0);

                _environment.SetActive(true);
                _environment.transform.DOScale(1f, 1f).SetEase(Ease.InFlash);

                _buttons.transform.position = mainCamera.transform.position + mainCamera.transform.forward * 1.5f;
                _buttons.transform.position = new Vector3(_buttons.transform.position.x, mainCamera.transform.position.y, _buttons.transform.position.z);
                _buttons.transform.LookAt(mainCamera.transform);
                _buttons.transform.rotation = Quaternion.Euler(0, _buttons.transform.rotation.eulerAngles.y, 0);

                _buttons.SetActive(true);
            }
        }
    }
}
