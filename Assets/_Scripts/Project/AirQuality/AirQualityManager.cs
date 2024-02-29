using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using GoNatureAR.Sensors;
using Microsoft.MixedReality.Toolkit.Utilities;
using Microsoft.MixedReality.Toolkit.UI;

namespace GoNatureAR
{
    public class AirQualityManager : MonoBehaviour
    {
        public static Action<AirIndex> OnAirQualityCalculated;
        public static Action<AirIndex> OnInfoButtonPressed;

        public static HashSet<string> AQIElements = new HashSet<string>
        {
            AirQualityElements.NO2.ToString(),
            AirQualityElements.PM2_5.ToString(),
            AirQualityElements.PM10.ToString(),
            AirQualityElements.SO2.ToString(),
            AirQualityElements.O3.ToString()
        };

        [SerializeField]
        private Interactable _infoButton;
        [SerializeField]
        private GameObject[] _menuAirQualityVFX;
        [SerializeField]
        private GridObjectCollection _handMenuGrid;

        [SerializeField]
        GameObject _badAirQualityDetails;
        [SerializeField]
        GameObject _goodAirQualityDetails;
        [SerializeField]
        TextMesh _badAirQualityDetailsText;
        [SerializeField]
        TextMesh _goodAirQualityDetailsText;

        private AirQualityIndex airQualityIndex;
        private int airQualityValue;
        private AirIndex airScenario;
        private bool hasTransfered = false;

        private void OnEnable()
        {
            GameManager.OnChangeState += ChangeStateHandle;
            NarrationManager.OnConfirmed += TransferToBadConditions;
        }

        private void OnDisable()
        {
            GameManager.OnChangeState -= ChangeStateHandle;
            NarrationManager.OnConfirmed -= TransferToBadConditions;
        }
        private void Start()
        {
            _infoButton.OnClick.AddListener(ShowAQIDetails);
        }

        /// <summary>
        /// Handles the state change event.
        /// </summary>
        /// <param name="state">The new state of the game.</param>
        /// <remarks>
        /// This method is called when the game state changes. If the new state is 'AirQuality', it calculates the air quality value based on the current air data.
        /// If air data is not available, it sets a default air quality value. Then, it determines the air quality scenario ('Good' or 'Unhealthy') based on the air quality value,
        /// gets the corresponding air quality index, and sets the scene based on the air quality scenario.
        /// </remarks>
        private void ChangeStateHandle(State state)
        {
            DisableVFX();
            if (state == State.AirQuality)
            {
                if (DataManager.Instance.AirData != null)
                {
                    CalculateAirQuality(
                        DataManager.Instance.AirData[AirQualityElements.PM2_5].Value,
                        DataManager.Instance.AirData[AirQualityElements.PM10].Value,
                        DataManager.Instance.AirData[AirQualityElements.NO2].Value,
                        DataManager.Instance.AirData[AirQualityElements.O3].Value,
                        DataManager.Instance.AirData[AirQualityElements.SO2].Value);
                }
                else
                    CalculateAirQuality(28, 55, 100, 155, 123);
            }
        }

        private void CalculateAirQuality(double PM2_5, double PM10, double NO2, double O3, double SO2)
        {
            airQualityValue = AirQualityCalculator.CalculateEU_AQI(PM2_5,PM10,NO2,O3,SO2);
            airQualityIndex = AirQualityCalculator.GetAirQualityIndex(airQualityValue);

            if (airQualityValue <= 1)
                airScenario = AirIndex.Good;
            else
                airScenario = AirIndex.Unhealthy;

            SetSceneBasedOnAirQuality();
            OnAirQualityCalculated?.Invoke(airScenario);
        }


        /// <summary>
        /// Sets the scene based on the given air quality scenario.
        /// </summary>
        /// <param name="scenario">The air quality scenario, which can be either 'Good' or 'Unhealthy'.</param>
        /// <remarks>
        /// This method updates the air quality details text for both good and bad air quality scenarios.
        /// If air data is available, it iterates over the data and updates the text and visual effects accordingly.
        /// If no air data is available, it sets default values for the air quality details text and activates the corresponding visual effects.
        /// After updating the air quality details and visual effects, it updates the hand menu grid and invokes the OnAirQualityCalculated event.
        /// </remarks>
        private void SetSceneBasedOnAirQuality()
        {
            _badAirQualityDetailsText.text = $"Air Quality: {airQualityIndex}\n";
            _goodAirQualityDetailsText.text = $"Air Quality: {airQualityIndex}\n";

            if (DataManager.Instance.AirData != null)
            {
                foreach (var item in DataManager.Instance.AirData)
                {
                    if (item.Value == 0)
                        continue;

                    SetDetailsText(item);
                    foreach (var vfx in _menuAirQualityVFX)
                    {
                        if (item.Key.ToString() == vfx.gameObject.name)
                            vfx.gameObject.SetActive(true);
                    }
                }
            }
            else
            {
                foreach (var vfx in _menuAirQualityVFX)
                {
                    if (AQIElements.Contains(vfx.gameObject.name))
                    {
                        BadQualityText(vfx.gameObject.name);
                        vfx.gameObject.SetActive(true);
                    }
                }
            }
            _handMenuGrid.UpdateCollection();
        }

        private void TransferToBadConditions()
        {
            _infoButton.IsToggled = false;
            _goodAirQualityDetails.SetActive(false);
            hasTransfered = true;
            CalculateAirQuality(28, 55, 100, 155, 123);
        }

        private void SetDetailsText(KeyValuePair<AirQualityElements, double?> item)
        {   
            if (hasTransfered && AQIElements.Contains(item.Key.ToString()))
            {
                BadQualityText(item.Key.ToString());
            }
            else
            {
                _badAirQualityDetailsText.text += $"{item.Key}: {((double)item.Value).ToString("F1")} ìg/m3\n";
                _goodAirQualityDetailsText.text += $"{item.Key}: {((double)item.Value).ToString("F1")} ìg/m3\n";
            }
        }

        private void BadQualityText(string name)
        {
            switch (name)
            {
                case "PM2_5":
                    _badAirQualityDetailsText.text += $"PM2_5: 28 ìg/m3\n";
                    break;
                case "PM10":
                    _badAirQualityDetailsText.text += $"PM10: 55 ìg/m3\n";
                    break;
                case "NO2":
                    _badAirQualityDetailsText.text += $"NO2: 100 ìg/m3\n";
                    break;
                case "O3":
                    _badAirQualityDetailsText.text += $"O3: 155 ìg/m3\n";
                    break;
                case "SO2":
                    _badAirQualityDetailsText.text += $"SO2: 123 ìg/m3\n";
                    break;
            }
        }

        private void DisableVFX()
        {
            foreach (var vfx in _menuAirQualityVFX)
                vfx.gameObject.SetActive(false);
        }

        /// <summary>
        /// Shows the air quality index details.
        /// Opens the right info panel based on air scenario.
        /// </summary>
        private void ShowAQIDetails()
        {
            switch(airScenario)
            {
                case AirIndex.Good:
                    _goodAirQualityDetails.SetActive(!_goodAirQualityDetails.activeSelf);
                    _infoButton.IsToggled = _goodAirQualityDetails.activeSelf;
                    _badAirQualityDetails.SetActive(false);
                    break;
                case AirIndex.Unhealthy:
                    _badAirQualityDetails.SetActive(!_badAirQualityDetails.activeSelf);
                    _infoButton.IsToggled = _badAirQualityDetails.activeSelf;
                    _goodAirQualityDetails.SetActive(false);
                    break;
            }

            if (!_infoButton.IsToggled)
                return;

            OnInfoButtonPressed?.Invoke(airScenario);
        }
    }
}
