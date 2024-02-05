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
            AirQualityData.Measurements = new AirQualityMeasurements[]
            {
                    AirQualityMeasurements.pm10_0,
                    AirQualityMeasurements.pm2_5,
                    AirQualityMeasurements.pm1_0
            };

            ThermalComfortData = new ThermalComfort();
            ThermalComfortData.Measurements = new ThermalComfortMeasurements[]
            {
                    ThermalComfortMeasurements.airTemperature,
                    ThermalComfortMeasurements.humidity
            };

            NoiseData = new Noise();
        }
    }
}
