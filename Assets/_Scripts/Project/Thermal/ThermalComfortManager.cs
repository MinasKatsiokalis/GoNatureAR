using GoNatureAR.Sensors;
using Microsoft.MixedReality.Toolkit.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GoNatureAR
{
    public class ThermalComfortManager : MonoBehaviour
    {
        public static Action<ThermalComofortIndex, double> OnThermalComfortCalculated;
        public static Action<ThermalComofortIndex> OnInfoButtonPressed;

        private double PMV;
        private int PPD;
        private ThermalComofortIndex Index;

        [SerializeField]
        Interactable _button;
        [SerializeField]
        GameObject _thermalDetailsPanel;
        [SerializeField]
        TextMesh _thermalDetailsText;

        private void OnEnable()
        {
            GameManager.OnChangeState += ChangeStateHandle;
            NarrationManager.OnConfirmed += TransferToHotConditions;
        }

        private void OnDisable()
        {
            GameManager.OnChangeState -= ChangeStateHandle;
            NarrationManager.OnConfirmed -= TransferToHotConditions;
        }

        private void Start()
        {
            _button.OnClick.AddListener(ShowThermalDetails);
        }

        private void CalculateThermalComfort(double temperature, double humidity)
        {
            PMV = ThermalComfortCalculator.CalculatePMV(temperature, humidity);
            PPD = ThermalComfortCalculator.CalculatePPD(PMV);
            Index = ThermalComfortCalculator.GetThermalComfortIndex(PMV);

            SetThermalDetails(temperature, humidity);
            OnThermalComfortCalculated?.Invoke(Index, humidity);
        }

        private void ChangeStateHandle(State state)
        {
            if (state == State.Temperature)
            {
                if (DataManager.Instance.ThermalData != null)
                    CalculateThermalComfort(DataManager.Instance.ThermalData[ThermalComfortElements.Temperature].Value, DataManager.Instance.ThermalData[ThermalComfortElements.Humidity].Value);
                else
                    CalculateThermalComfort(25, 55);
            }
        }

        private void SetThermalDetails(double temperature, double humidity)
        {
            _thermalDetailsText.text = String.Format("Thermal Condition: {0}  \n\nTemperature: {1}C \n\nHumidity: {2}% \n\nPredicted Percentage\nof Dissatisfied (PPD): {3}%", Index, temperature, humidity, PPD);
        }

        private void ShowThermalDetails()
        {
            _thermalDetailsPanel.SetActive(!_thermalDetailsPanel.activeSelf);
            _button.IsToggled = _thermalDetailsPanel.activeSelf;

            if(Index == ThermalComofortIndex.Hot)
                _thermalDetailsPanel.transform.GetChild(1).gameObject.SetActive(_thermalDetailsPanel.activeSelf);

            if (!_thermalDetailsPanel.activeSelf)
                return;

            if(Index == ThermalComofortIndex.Cold || Index == ThermalComofortIndex.Comfortable)
                OnInfoButtonPressed?.Invoke(Index);
            else
                OnInfoButtonPressed?.Invoke(ThermalComofortIndex.Hot);
        }

        private void TransferToHotConditions()
        {
            _thermalDetailsPanel.SetActive(false);
            _button.IsToggled = false;
            CalculateThermalComfort(30, 70);
        }
    }

    public class ThermalColors
    {
        public Color Comfortable;
        public Color Cold;
        public Color Hot;
        public ThermalColors()
        {
            Comfortable = new Color(0.0f, 1.0f, 0.0f, 0.78f);
            Cold = new Color(1.0f, 1.0f, 1.0f, 0.78f);
            Hot = new Color(1.0f, 0.0f, 0.0f, 0.78f);
        }
    }
}
