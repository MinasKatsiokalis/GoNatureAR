using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GoNatureAR.Sensors
{
    public class CastelfrancoVenetoSensorData : ISensorData
    {
        public AirQuality AirQualityData { get; }
        public ThermalComfort ThermalComfortData { get; }
        public Noise NoiseData { get; }

        public CastelfrancoVenetoSensorData()
        {
            AirQualityData = new AirQuality();
            AirQualityData.Measurements = new AirQualityMeasurements[]
            {
                    AirQualityMeasurements.MassConcentrationPM10_0,
                    AirQualityMeasurements.MassConcentrationPM2_5,
                    AirQualityMeasurements.MassConcentrationPM1_0,
                    AirQualityMeasurements.MassConcentrationPM4_0,
                    AirQualityMeasurements.CO2ppm,
                    AirQualityMeasurements.NO2ugm3,
                    AirQualityMeasurements.O3ppb
            };

            ThermalComfortData = new ThermalComfort();
            ThermalComfortData.Measurements = new ThermalComfortMeasurements[]
            {
                    ThermalComfortMeasurements.airTemperature,
                    ThermalComfortMeasurements.humidity
            };

            NoiseData = new Noise();
            NoiseData.Measurements = new NoiseMeasurements[]
            {
                    NoiseMeasurements.LAEQ,
                    NoiseMeasurements.dba_min,
                    NoiseMeasurements.dba_max,
            };
        }
    }
}
