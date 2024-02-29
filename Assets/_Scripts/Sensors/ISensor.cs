using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoNatureAR;
namespace GoNatureAR.Sensors
{
    public interface ISensor
    {
        public IReadOnlyDictionary<SensorType, List<string>> SensorTypes { get; }
    }

    public class ChaniaSensors : ISensor
    {
        private Dictionary<SensorType, List<string>> sensorTypes = new Dictionary<SensorType, List<string>>() 
        {
            {SensorType.air, new List<string> { "cyclosensor_station" } },
            {SensorType.thermal, new List<string> { "meteo_gr", "sensedge_stick" } },
            {SensorType.noise, new List<string> { "cyclosensor_station","cyclosensor_bike" } }
        };
        public IReadOnlyDictionary<SensorType, List<string>> SensorTypes => sensorTypes;
    }

    public class NovoMestoSensors : ISensor
    {
        private Dictionary<SensorType, List<string>> sensorTypes = new Dictionary<SensorType, List<string>>() 
        {
            {SensorType.air, new List<string> { "airly" } },
            {SensorType.thermal, new List<string> { "sloveniaWeatherStationXML", "sensedge_stick" } }
        };
        public IReadOnlyDictionary<SensorType, List<string>> SensorTypes => sensorTypes;
    }

    public class LeuvenSensors : ISensor
    {
        private Dictionary<SensorType, List<string>> sensorTypes = new Dictionary<SensorType, List<string>>() 
        {
            {SensorType.air, new List<string> { "lw12terpm" } },
            {SensorType.thermal, new List<string> { "WH2600", "sensedge_stick", "lw12terpm" } }
        };
        public IReadOnlyDictionary<SensorType, List<string>> SensorTypes => sensorTypes;
    }

    public class SkellefteaSensors : ISensor
    {
        private Dictionary<SensorType, List<string>> sensorTypes = new Dictionary<SensorType, List<string>>() 
        {
            {SensorType.thermal, new List<string> { "iot_open_sensedge_stick" } }
        };
        public IReadOnlyDictionary<SensorType, List<string>> SensorTypes => sensorTypes;
    }

    public class CastelfrancoSensors : ISensor
    {
        private Dictionary<SensorType, List<string>> sensorTypes = new Dictionary<SensorType, List<string>>() 
        {
            {SensorType.air, new List<string> { "synetica-enl-air-x" } },
            {SensorType.thermal, new List<string> { "sensedge_stick", "synetica-enl-air-x" } },
            {SensorType.noise, new List<string> { "iotsense-noise" } }
        };
        public IReadOnlyDictionary<SensorType, List<string>> SensorTypes => sensorTypes;
    }

    public class DundalkSensors : ISensor
    {
        private Dictionary<SensorType, List<string>> sensorTypes = new Dictionary<SensorType, List<string>>() 
        {
            {SensorType.air, new List<string> { "airquality_ie", "enlink-airx" } },
            {SensorType.thermal, new List<string> { "sensedge_stick", "enlink-airx" } },
            {SensorType.noise, new List<string> { "polysense-wxs8800" } }
        };
        public IReadOnlyDictionary<SensorType, List<string>> SensorTypes => sensorTypes;
    }

    public class GziraSensors : ISensor
    {
        private Dictionary<SensorType, List<string>> sensorTypes = new Dictionary<SensorType, List<string>>() 
        {
            {SensorType.air, new List<string> { "aq-senstate-uaqm" } },
            {SensorType.thermal, new List<string> { "sensedge_stick" } }
        };
        public IReadOnlyDictionary<SensorType, List<string>> SensorTypes => sensorTypes;
    }
}