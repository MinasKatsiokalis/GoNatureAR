using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GoNatureAR.Sensors
{
    public class DundalkSensorData : ISensorData
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
                    AirQualityMeasure.MassConcentrationPM1_0,
                    AirQualityMeasure.MassConcentrationPM4_0,
                    AirQualityMeasure.COugm3,
                    AirQualityMeasure.NO2ugm3,
                    AirQualityMeasure.SO2ugm3,
                    AirQualityMeasure.CO2ppm
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
            set 
            { 
                NoiseData = new Noise();
                NoiseData.Measures = new NoiseMeasure[]
                {
                    NoiseMeasure.sounddB
                };
            }
        }
    }
}
