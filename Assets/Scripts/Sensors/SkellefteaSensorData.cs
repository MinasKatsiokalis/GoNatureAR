using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GoNatureAR.Sensors
{
    public class SkellefteaSensorData : ISensorData
    {
        public AirQuality AirQualityData
        {
            get { return AirQualityData; }
            set { AirQualityData = new AirQuality(); }
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
