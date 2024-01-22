using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GoNatureAR.Sensors
{
    public interface ISensorData
    {
        public AirQuality AirQualityData { get; }
        public ThermalComfort ThermalComfortData { get; }
        public Noise NoiseData { get; }
    }

    [Serializable]
    public class AirQuality
    {
        public AirQualityMeasure[] Measurements;
    }

    [Serializable]
    public class ThermalComfort
    {
        public ThermalComfortMeasure[] Measurements;
    }

    public class Noise
    {
        public NoiseMeasure[] Measurements;
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
        O3ppb
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
        sounddB,
        dba_max,
        dba_min,
        LAEQ
    }
}
