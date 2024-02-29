using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GoNatureAR.Sensors
{
    public class NovoMestoSensorData : ISensorData
    {
        public AirQuality AirQualityData { get; }
        public ThermalComfort ThermalComfortData { get; }
        public Noise NoiseData { get; }

        public NovoMestoSensorData()
        {
            AirQualityData = new AirQuality();
            AirQualityData.Measurements = new AirQualityMeasurements[]
            {
                    AirQualityMeasurements.MassConcentrationPM10_0,
                    AirQualityMeasurements.MassConcentrationPM2_5,
                    AirQualityMeasurements.MassConcentrationPM1_0
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
