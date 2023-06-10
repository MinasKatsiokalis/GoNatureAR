using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        // Example concentration values
        double PM2_5 = 20; // µg/m³
        double PM10 = 40; // µg/m³
        double NO2 = 50; // µg/m³
        double O3 = 60; // µg/m³
        double CO2 = 3500; // µg/m³

        // Calculate the EU-AQI
        
        Debug.Log(AirQualityCalculator.CalculateEU_AQI(PM2_5, PM10, NO2, O3, CO2));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
