using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Test : MonoBehaviour
{
    // Start is called before the first frame update
    private string sensor = "urn:ngsi-ld:synetica-enl-air-x:synetica-enl-air-x-004815";
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
        PilotAuthentication(Pilot.castelfranco_veneto.ToString());

    }
    async void PilotAuthentication(string pilot)
    {
        string accessToken = await AuthenticationManager.GetAccessToken(pilot);
        AuthenticationManager.GetData(accessToken, pilot);
        AuthenticationManager.GetSensorData(accessToken, pilot, sensor);
    }

}
