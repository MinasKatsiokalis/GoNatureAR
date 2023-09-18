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
        double PM2_5 = 20; // �g/m�
        double PM10 = 40; // �g/m�
        double NO2 = 50; // �g/m�
        double O3 = 60; // �g/m�
        double CO2 = 3500; // �g/m�

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
