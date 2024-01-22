using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using Newtonsoft.Json.Linq;
using GoNatureAR.Requests;
using GoNatureAR.Sensors;
using GoNatureAR;

public class Test : MonoBehaviour
{
    [SerializeField]
    Pilot pilot;
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
        PilotAuthentication(pilot);
    }
    void PilotAuthentication(Pilot pilot)
    {
        PilotDataRequestManager.Instance.SendRequestsForData(pilot);
    }
}
