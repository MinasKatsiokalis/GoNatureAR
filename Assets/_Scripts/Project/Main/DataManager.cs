using GoNatureAR.Requests;
using GoNatureAR.Sensors;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GoNatureAR
{
    public class DataManager : MonoBehaviour
    {
        public static DataManager Instance { private set; get; }

        public static Action OnDataUpdated;

        public Pilot Pilot { private set; get; }
        public Dictionary<ThermalComfortElements, double?> ThermalData { private set; get; }
        public Dictionary<AirQualityElements, double?> AirData { private set; get; }
        public Dictionary<NoiseElements, double?> NoiseData { private set; get; }

        private void Awake()
        {
            if (Instance == null)
                Instance = this;
            else
                Destroy(this.gameObject);
        }

        private void OnEnable()
        {
            PilotDataRequestManager.OnPilotSelect += SetPilot;
            PilotDataRequestManager.OnThermalDataRecieved += SetPilotThermalData;
            PilotDataRequestManager.OnAirDataRecieved += SetPilotAirData;
            PilotDataRequestManager.OnNoiseDataRecieved += SetNoisePilotData;
            PilotDataRequestManager.OnDataReceived += () => OnDataUpdated?.Invoke();
            PilotDataRequestManager.OnError += ResetData;
        }

        private void OnDisable()
        {
            PilotDataRequestManager.OnPilotSelect -= SetPilot;
            PilotDataRequestManager.OnThermalDataRecieved -= SetPilotThermalData;
            PilotDataRequestManager.OnAirDataRecieved -= SetPilotAirData;
            PilotDataRequestManager.OnNoiseDataRecieved -= SetNoisePilotData;
            PilotDataRequestManager.OnError -= ResetData;
        }

        private void SetPilot(Pilot pilot)
        {
            Pilot = pilot;
        }

        #region Thermal Data
        private void SetPilotThermalData(Dictionary<EnumKey, double?> thermalData)
        {
            if (thermalData == null)
            {
                ThermalData = null;
                return;
            }
            EnumKey airTemperatureKey = new EnumKey(ThermalComfortMeasurements.airTemperature);
            EnumKey humidityKey = new EnumKey(ThermalComfortMeasurements.humidity);

            ThermalData = new Dictionary<ThermalComfortElements, double?>();
            ThermalData[ThermalComfortElements.Temperature] = thermalData.ContainsKey(airTemperatureKey) ? thermalData[airTemperatureKey] : 0;
            ThermalData[ThermalComfortElements.Humidity] = thermalData.ContainsKey(humidityKey) ? thermalData[humidityKey] : 0;
        }
        #endregion

        #region Air Data
        private void SetPilotAirData(Dictionary<EnumKey, double?> airData)
        {   
            if (airData == null)
            {
                AirData = null;
                return;
            }

            AirData = new Dictionary<AirQualityElements, double?>();
            AirData[AirQualityElements.PM10] = GetAirDataValue(airData, AirQualityMeasurements.MassConcentrationPM10_0, AirQualityMeasurements.pm10_0);
            AirData[AirQualityElements.PM2_5] = GetAirDataValue(airData, AirQualityMeasurements.MassConcentrationPM2_5, AirQualityMeasurements.pm2_5);
            AirData[AirQualityElements.PM1] = GetAirDataValue(airData, AirQualityMeasurements.MassConcentrationPM1_0, AirQualityMeasurements.pm1_0);
            AirData[AirQualityElements.PM4] = GetAirDataValue(airData, AirQualityMeasurements.MassConcentrationPM4_0);
            AirData[AirQualityElements.CL2] = GetAirDataValue(airData, AirQualityMeasurements.CL2ppm, molecularWeight: 70.906, isPpm: true);
            AirData[AirQualityElements.CO2] = GetAirDataValue(airData, AirQualityMeasurements.CO2ppm, molecularWeight: 44.01, isPpm: true);
            AirData[AirQualityElements.CO] = GetAirDataValue(airData, AirQualityMeasurements.COppm, AirQualityMeasurements.COugm3, 28.01, true);
            AirData[AirQualityElements.NH3] = GetAirDataValue(airData, AirQualityMeasurements.NH3ppm, molecularWeight: 17.031, isPpm: true);
            AirData[AirQualityElements.NO2] = GetAirDataValue(airData, AirQualityMeasurements.NO2ppm, AirQualityMeasurements.NO2ugm3, 46.0055, true);
            AirData[AirQualityElements.NO] = GetAirDataValue(airData, AirQualityMeasurements.NOppm, molecularWeight: 30.0061, isPpm: true);
            AirData[AirQualityElements.SO2] = GetAirDataValue(airData, AirQualityMeasurements.SO2ppm, AirQualityMeasurements.SO2ugm3, 64.066, true);
            AirData[AirQualityElements.O3] = GetAirDataValue(airData, AirQualityMeasurements.O3ppb, molecularWeight: 48.0, isPpb: true);
        }

        private double GetAirDataValue(Dictionary<EnumKey, double?> airData, AirQualityMeasurements primaryMeasurement, AirQualityMeasurements? secondaryMeasurement = null, double? molecularWeight = null, bool isPpm = false, bool isPpb = false)
        {
            EnumKey primaryKey = new EnumKey(primaryMeasurement);
            EnumKey secondaryKey = secondaryMeasurement.HasValue ? new EnumKey(secondaryMeasurement.Value) : null;

            if (airData.ContainsKey(primaryKey))
            {
                if (molecularWeight.HasValue)
                {
                    if (isPpm)
                        return PpmToMicrogramsPerCubicMeter(airData[primaryKey].Value, molecularWeight.Value);
                    else if (isPpb)
                        return PpbToMicrogramsPerCubicMeter(airData[primaryKey].Value, molecularWeight.Value);
                }
                return airData[primaryKey].Value;
            }
            else if (secondaryKey != null && airData.ContainsKey(secondaryKey))
                return airData[secondaryKey].Value;
            else
                return 0;
        }

        private double PpmToMicrogramsPerCubicMeter(double ppm, double molecularWeight)
        {
            const double standardTemperatureInKelvin = 298.15; // 25 degrees Celsius in Kelvin
            const double standardPressureInAtm = 1; // 1 atmosphere
            const double universalGasConstant = 0.0821; // in L*atm/(K*mol)

            // Convert ppm to moles per liter (molarity)
            double molarity = ppm * 1e-6;

            // Convert molarity to micrograms per cubic meter
            double microgramsPerCubicMeter = molarity * standardPressureInAtm * molecularWeight * 1e6 / (universalGasConstant * standardTemperatureInKelvin);

            return microgramsPerCubicMeter;
        }

        private double PpbToMicrogramsPerCubicMeter(double ppb, double molecularWeight)
        {
            const double standardTemperatureInKelvin = 298.15; // 25 degrees Celsius in Kelvin
            const double standardPressureInAtm = 1; // 1 atmosphere
            const double universalGasConstant = 0.0821; // in L*atm/(K*mol)

            // Convert ppb to moles per liter (molarity)
            double molarity = ppb * 1e-9;

            // Convert molarity to micrograms per cubic meter
            double microgramsPerCubicMeter = molarity * standardPressureInAtm * molecularWeight * 1e6 / (universalGasConstant * standardTemperatureInKelvin);

            return microgramsPerCubicMeter;
        }
        #endregion

        #region Noise Data
        private void SetNoisePilotData(Dictionary<EnumKey, double?> noiseData)
        {   
            if(noiseData == null)
            {
                NoiseData = null;
                return;
            }

            EnumKey soundDbKey = new EnumKey(NoiseMeasurements.sounddB);
            EnumKey soundKey = new EnumKey(NoiseMeasurements.sound);
            EnumKey laeqKey = new EnumKey(NoiseMeasurements.LAEQ);

            NoiseData = new Dictionary<NoiseElements, double?>();
            NoiseData[NoiseElements.Sound] = noiseData.ContainsKey(soundDbKey) ? noiseData[soundDbKey] :
                                             noiseData.ContainsKey(soundKey) ? noiseData[soundKey] :
                                             noiseData.ContainsKey(laeqKey) ? noiseData[laeqKey] : 0;
        }
        #endregion

        private void ResetData(string error)
        {
            ThermalData = null;
            AirData = null;
            NoiseData = null;
        }
    }

    public enum AirQualityElements
    {
        PM10,
        PM2_5,
        PM1,
        PM4,
        CL2,
        CO2,
        CO,
        NH3,
        NO2,
        NO,
        SO2,
        O3
    }

    public enum ThermalComfortElements
    {
        Temperature,
        Humidity
    }

    public enum NoiseElements
    {
        Sound
    }
}
