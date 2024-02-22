using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoNatureAR.Sensors;
using System;

namespace GoNatureAR
{
    public class AirQualityManager : MonoBehaviour
    {
        public static Action<AirIndex> OnAirQualityCalculated;

        private AirQualityIndex airQualityIndex;
        private int airQualityValue;
        private AirIndex airScenario;

        private void OnEnable()
        {
            GameManager.OnChangeState += ChangeStateHandle;
            //NarrationManager.OnConfirmed += TransferToLoudConditions;
        }

        private void OnDisable()
        {
            GameManager.OnChangeState -= ChangeStateHandle;
            //NarrationManager.OnConfirmed -= TransferToLoudConditions;
        }

        // Start is called before the first frame update
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
        
        }

        private void ChangeStateHandle(State state)
        {
            if (state == State.AirQuality)
            {
                if (DataManager.Instance.AirData != null)
                {
                    airQualityValue = AirQualityCalculator.CalculateEU_AQI(
                       DataManager.Instance.AirData[AirQualityElements.PM2_5].Value,
                       DataManager.Instance.AirData[AirQualityElements.PM10].Value,
                       DataManager.Instance.AirData[AirQualityElements.NO2].Value,
                       DataManager.Instance.AirData[AirQualityElements.O3].Value,
                       DataManager.Instance.AirData[AirQualityElements.SO2].Value
                    );
                }
                else
                    airQualityValue = 3;
                if(airQualityValue <= 1)
                    airScenario = AirIndex.Good;
                else
                    airScenario = AirIndex.Unhealthy;

                airQualityIndex = AirQualityCalculator.GetAirQualityIndex(airQualityValue);
                SetSceneBasedOnAirQuality(airScenario);
            }
        }

        private void SetSceneBasedOnAirQuality(AirIndex scenario)
        {
            foreach (var item in DataManager.Instance.AirData)
            {
                if(item.Value > 0)
                    Debug.Log(item.Key + " : " + item.Value);
            }
        }
    }
}
