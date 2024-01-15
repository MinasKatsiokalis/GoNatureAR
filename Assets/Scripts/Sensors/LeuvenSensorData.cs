using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GoNatureAR.Sensors
{
    public class LeuvenSensorData : ISensorData
    {
        public AirQuality AirQualityData
        {
            get { return AirQualityData; }
            set
            {
                AirQualityData = new AirQuality();
                AirQualityData.Measures = new AirQualityMeasure[]
                {
                    AirQualityMeasure.pm10_0,
                    AirQualityMeasure.pm2_5,
                    AirQualityMeasure.pm1_0
                };
            }
        }

        public ThermalComfort ThermalComfortData
        {
            get { return ThermalComfortData; }
            set
            {
                ThermalComfortData = new ThermalComfort();
                ThermalComfortData.Measures = new ThermalComfortMeasure[]
                {
                    ThermalComfortMeasure.airTemperature,
                    ThermalComfortMeasure.humidity
                };
            }
        }

        public Noise NoiseData
        {
            get { return NoiseData; }
            set { NoiseData = new Noise(); }
        }
    }
}
