using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using Newtonsoft.Json.Linq;
using GoNatureAR.Requests;
using GoNatureAR.Sensors;
using GoNatureAR;

public class Test : MonoBehaviour
{
    [SerializeField]
    Pilot pilot;

    private void OnEnable()
    {
        //DataManager.OnDataUpdated += SetData;    
    }

    private void OnDisable()
    {
        //DataManager.OnDataUpdated -= SetData;
    }

    void Start()
    {

    }

    private void SetData(SensorType dataType)
    {
        switch (dataType)
        {
            case SensorType.air:
                {
                    if (DataManager.Instance.AirData == null)
                        return;

                    foreach (var measurement in DataManager.Instance.AirData.Keys)
                    {
                        Debug.Log(measurement.ToString() + " : " + DataManager.Instance.AirData[measurement].Value);
                    }

                    var EU_AQI = AirQualityCalculator.CalculateEU_AQI(
                        DataManager.Instance.AirData[AirQualityElements.PM10].Value,
                        DataManager.Instance.AirData[AirQualityElements.PM10].Value,
                        DataManager.Instance.AirData[AirQualityElements.NO2].Value,
                        DataManager.Instance.AirData[AirQualityElements.O3].Value,
                        DataManager.Instance.AirData[AirQualityElements.SO2].Value
                    );
                    var index = AirQualityCalculator.GetAirQualityIndex(EU_AQI);

                    Debug.Log("EU_AQI: " + EU_AQI);
                    Debug.Log("Index: " + index);   
                }
                break;
            case SensorType.noise:
                {
                    if (DataManager.Instance.NoiseData == null)
                        return;

                    var noise = DataManager.Instance.NoiseData[NoiseElements.Sound].Value;
                    Debug.Log("Noise: " + noise);
                }
                break;
        } 
    }
}
