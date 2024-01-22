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
            AirQualityData.Measurements = new AirQualityMeasure[]
            {
                    AirQualityMeasure.MassConcentrationPM10_0,
                    AirQualityMeasure.MassConcentrationPM2_5,
                    AirQualityMeasure.MassConcentrationPM1_0,
                    AirQualityMeasure.CL2ppm,
                    AirQualityMeasure.CO2ppm,
                    AirQualityMeasure.COppm,
                    AirQualityMeasure.NH3ppm,
                    AirQualityMeasure.NO2ppm,
                    AirQualityMeasure.Noppm,
                    AirQualityMeasure.SO2ppm
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