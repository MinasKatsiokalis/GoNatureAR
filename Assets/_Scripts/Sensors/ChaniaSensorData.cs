using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GoNatureAR.Sensors
{
    public class ChaniaSensorData : ISensorData
    {
        public AirQuality AirQualityData { get; }
        public ThermalComfort ThermalComfortData { get; }
        public Noise NoiseData { get; }

        public ChaniaSensorData()
        {
            AirQualityData = new AirQuality();
            AirQualityData.Measurements = new AirQualityMeasurements[]
            {
                    AirQualityMeasurements.MassConcentrationPM10_0,
                    AirQualityMeasurements.MassConcentrationPM2_5,
                    AirQualityMeasurements.MassConcentrationPM1_0,
                    AirQualityMeasurements.CL2ppm,
                    AirQualityMeasurements.CO2ppm,
                    AirQualityMeasurements.COppm,
                    AirQualityMeasurements.NH3ppm,
                    AirQualityMeasurements.NO2ppm,
                    AirQualityMeasurements.NOppm,
                    AirQualityMeasurements.SO2ppm
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
                    NoiseMeasurements.sound,
            };
        }
    }
}