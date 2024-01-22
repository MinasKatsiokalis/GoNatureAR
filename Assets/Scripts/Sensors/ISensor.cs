using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GoNatureAR.Sensors
{
    public interface ISensor
    {
        public IReadOnlyDictionary<string, List<string>> SensorTypes { get; }
    }

    public class ChaniaSensors : ISensor
    {
        private Dictionary<string, List<string>> sensorTypes = new Dictionary<string, List<string>>();
        public IReadOnlyDictionary<string, List<string>> SensorTypes => sensorTypes;

        public ChaniaSensors()
        {
            sensorTypes = new Dictionary<string, List<string>>() {
                 {"air", new List<string> { "cyclosensor_station" } },
                 {"thermal", new List<string> { "meteo_gr", "sensedge_stick" } }
            };
        }
    }

    public class NovoMestoSensors : ISensor
    {
        private Dictionary<string, List<string>> sensorTypes = new Dictionary<string, List<string>>();
        public IReadOnlyDictionary<string, List<string>> SensorTypes => sensorTypes;

        public NovoMestoSensors()
        {
            sensorTypes = new Dictionary<string, List<string>>() {
                 {"air", new List<string> { "airly" } },
                 {"thermal", new List<string> { "sloveniaWeatherStationXML", "sensedge_stick" } }
            };
        }
    }

    public class LeuvenSensors : ISensor
    {
        private Dictionary<string, List<string>> sensorTypes = new Dictionary<string, List<string>>();
        public IReadOnlyDictionary<string, List<string>> SensorTypes => sensorTypes;

        public LeuvenSensors()
        {
            sensorTypes = new Dictionary<string, List<string>>() {
                 {"air", new List<string> { "lw12terpm" } },
                 {"thermal", new List<string> { "WH2600", "sensedge_stick", "lw12terpm" } }
            };
        }
    }

    public class SkellefteaSensors : ISensor
    {
        private Dictionary<string, List<string>> sensorTypes = new Dictionary<string, List<string>>();
        public IReadOnlyDictionary<string, List<string>> SensorTypes => sensorTypes;

        public SkellefteaSensors()
        {
            sensorTypes = new Dictionary<string, List<string>>() {
                 {"thermal", new List<string> { "iot_open_sensedge_stick" } }
            };
        }
    }

    public class CastelfrancoSensors : ISensor
    {
        private Dictionary<string, List<string>> sensorTypes = new Dictionary<string, List<string>>();
        public IReadOnlyDictionary<string, List<string>> SensorTypes => sensorTypes;

        public CastelfrancoSensors()
        {
            sensorTypes = new Dictionary<string, List<string>>() {
                 {"air", new List<string> { "synetica-enl-air-x" } },
                 {"thermal", new List<string> { "sensedge_stick", "synetica-enl-air-x" } },
                 {"noise", new List<string> { "iotsense-noise" } }
            };
        }
    }

    public class DundalkSensors : ISensor
    {
        private Dictionary<string, List<string>> sensorTypes = new Dictionary<string, List<string>>();
        public IReadOnlyDictionary<string, List<string>> SensorTypes => sensorTypes;

        public DundalkSensors()
        {
            sensorTypes = new Dictionary<string, List<string>>() {
                 {"air", new List<string> { "airquality_ie", "enlink-airx" } },
                 {"thermal", new List<string> { "sensedge_stick", "enlink-airx" } },
                 {"noise", new List<string> { "polysense-wxs8800" } }
            };
        }
    }

    public class GziraSensors : ISensor
    {
        private Dictionary<string, List<string>> sensorTypes = new Dictionary<string, List<string>>();
        public IReadOnlyDictionary<string, List<string>> SensorTypes => sensorTypes;

        public GziraSensors()
        {
            sensorTypes = new Dictionary<string, List<string>>() {
                 {"air", new List<string> { "aq-senstate-uaqm" } },
                 {"thermal", new List<string> { "sensedge_stick" } }
            };
        }
    }

}