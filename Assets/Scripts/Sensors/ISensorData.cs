using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GoNatureAR.Sensors
{
    public interface ISensorData
    {
        AirQuality AirQualityData { set; get; }
        ThermalComfort ThermalComfortData { set; get; }
        Noise NoiseData { set; get; }
    }

    [Serializable]
    public class AirQuality
    {
        public AirQualityMeasure[] Measures;
    }

    [Serializable]
    public class ThermalComfort
    {
        public ThermalComfortMeasure[] Measures;
    }

    public class Noise
    {
        public NoiseMeasure[] Measures;
    }

    [Serializable]
    public enum AirQualityMeasure
    {
        MassConcentrationPM10_0,
        MassConcentrationPM2_5,
        MassConcentrationPM1_0,
        MassConcentrationPM4_0,
        pm10_0,
        pm2_5,
        pm1_0,
        CL2ppm,
        CO2ppm,
        COppm,
        NH3ppm,
        NO2ppm,
        Noppm,
        SO2ppm,
        COugm3,
        NO2ugm3,
        SO2ugm3,
    }

    [Serializable]
    public enum ThermalComfortMeasure
    {
        airTemperature,
        humidity
    }

    [Serializable]
    public enum NoiseMeasure
    {
        sounddB
    }
}
