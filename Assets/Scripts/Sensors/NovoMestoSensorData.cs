using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GoNatureAR.Sensors
{
    public class NovoMestoSensorData : ISensorData
    {
        public AirQuality AirQualityData
        {
            get { return AirQualityData; }
            set
            {
                AirQualityData = new AirQuality();
                AirQualityData.Measures = new AirQualityMeasure[]
                {
                    AirQualityMeasure.MassConcentrationPM10_0,
                    AirQualityMeasure.MassConcentrationPM2_5,
                    AirQualityMeasure.MassConcentrationPM1_0
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
