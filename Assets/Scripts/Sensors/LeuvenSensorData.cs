using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GoNatureAR.Sensors
{
    public class LeuvenSensorData : ISensorData
    {
        public AirQuality AirQualityData { get; }
        public ThermalComfort ThermalComfortData { get; }
        public Noise NoiseData { get; }

        public LeuvenSensorData()
        {
            AirQualityData = new AirQuality();
            AirQualityData.Measurements = new AirQualityMeasure[]
            {
                    AirQualityMeasure.pm10_0,
                    AirQualityMeasure.pm2_5,
                    AirQualityMeasure.pm1_0
            };

            ThermalComfortData = new ThermalComfort();
            ThermalComfortData.Measurements = new ThermalComfortMeasure[]
            {
                    ThermalComfortMeasure.airTemperature,
                    ThermalComfortMeasure.humidity
            };

            NoiseData = new Noise();
        }
    }
}
