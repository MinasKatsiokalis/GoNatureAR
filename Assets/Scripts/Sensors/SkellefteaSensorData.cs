using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GoNatureAR.Sensors
{
    public class SkellefteaSensorData : ISensorData
    {
        public AirQuality AirQualityData { get; }
        public ThermalComfort ThermalComfortData { get; }
        public Noise NoiseData { get; }

        public SkellefteaSensorData()
        {
            AirQualityData = new AirQuality();

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
