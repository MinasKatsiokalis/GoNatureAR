using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GoNatureAR.Sensors
{
    public class DundalkSensorData : ISensorData
    {
        public AirQuality AirQualityData { get; }
        public ThermalComfort ThermalComfortData { get; }
        public Noise NoiseData { get; }

        public DundalkSensorData()
        {
            AirQualityData = new AirQuality();
            AirQualityData.Measurements = new AirQualityMeasure[]
            {
                    AirQualityMeasure.MassConcentrationPM10_0,
                    AirQualityMeasure.MassConcentrationPM2_5,
                    AirQualityMeasure.MassConcentrationPM1_0,
                    AirQualityMeasure.MassConcentrationPM4_0,
                    AirQualityMeasure.COugm3,
                    AirQualityMeasure.NO2ugm3,
                    AirQualityMeasure.SO2ugm3,
                    AirQualityMeasure.CO2ppm
            };

            ThermalComfortData = new ThermalComfort();
            ThermalComfortData.Measurements = new ThermalComfortMeasure[]
            {
                    ThermalComfortMeasure.airTemperature,
                    ThermalComfortMeasure.humidity
            };

            NoiseData = new Noise();
            NoiseData.Measurements = new NoiseMeasure[]
            {
                    NoiseMeasure.sounddB
            };
        }
    }
}
