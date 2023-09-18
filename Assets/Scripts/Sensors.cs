using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sensors: MonoBehaviour
{
    private void Awake()
    {
        ImportSensorData();
    }

    private void ImportSensorData()
    {
        //Castelfranco
        //Air
        SensorData.AddSensor(Pilot.castelfranco_veneto, SensorType.air, "urn:ngsi-ld:synetica-enl-air-x:synetica-enl-air-x-004815");
        SensorData.AddSensor(Pilot.castelfranco_veneto, SensorType.air, "urn:ngsi-ld:synetica-enl-air-x:synetica-enl-air-x-004816");
        SensorData.AddSensor(Pilot.castelfranco_veneto, SensorType.air, "urn:ngsi-ld:synetica-enl-air-x:synetica-enl-air-x-004817");
        //Noise
        SensorData.AddSensor(Pilot.castelfranco_veneto, SensorType.noise, "urn:ngsi-ld:iotsense-noise:iotsense-noise-0004a30b0050be49");
        //Temperature
        SensorData.AddSensor(Pilot.castelfranco_veneto, SensorType.temperature, "urn:ngsi-ld:sensedge_stick:sensedge-01");
        SensorData.AddSensor(Pilot.castelfranco_veneto, SensorType.temperature, "urn:ngsi-ld:sensedge_stick:sensedge-02");
        SensorData.AddSensor(Pilot.castelfranco_veneto, SensorType.temperature, "urn:ngsi-ld:sensedge_stick:sensedge-03");
        SensorData.AddSensor(Pilot.castelfranco_veneto, SensorType.temperature, "urn:ngsi-ld:sensedge_stick:sensedge-04");
        SensorData.AddSensor(Pilot.castelfranco_veneto, SensorType.temperature, "urn:ngsi-ld:sensedge_stick:sensedge-05");
        SensorData.AddSensor(Pilot.castelfranco_veneto, SensorType.temperature, "urn:ngsi-ld:sensedge_stick:sensedge-06");
        SensorData.AddSensor(Pilot.castelfranco_veneto, SensorType.temperature, "urn:ngsi-ld:sensedge_stick:sensedge-07");
        SensorData.AddSensor(Pilot.castelfranco_veneto, SensorType.temperature, "urn:ngsi-ld:sensedge_stick:sensedge-08");
        SensorData.AddSensor(Pilot.castelfranco_veneto, SensorType.temperature, "urn:ngsi-ld:sensedge_stick:sensedge-09");
        SensorData.AddSensor(Pilot.castelfranco_veneto, SensorType.temperature, "urn:ngsi-ld:sensedge_stick:sensedge-10");
    }

    private void Start()
    {
        List<string> sensorsID = SensorData.GetSensors(Pilot.castelfranco_veneto, SensorType.temperature);
        foreach (var item in sensorsID)
        {
            Debug.Log(item.ToString());
        }
    }
}

public class SensorKey
{
    public Pilot Pilot { get; set; }
    public SensorType SensorType { get; set; }

    public SensorKey(Pilot pilot, SensorType type)
    {
        Pilot = pilot;
        SensorType = type;
    }

    public override bool Equals(object obj)
    {
        if (obj is SensorKey other)
        {
            return Pilot == other.Pilot && SensorType == other.SensorType;
        }
        return false;
    }

    public override int GetHashCode()
    {
        return Pilot.GetHashCode() ^ SensorType.GetHashCode();
    }
}

public static class SensorData
{
    public static Dictionary<SensorKey, List<string>> SensorDictionary = new Dictionary<SensorKey, List<string>>();

    public static void AddSensor(Pilot city, SensorType type, string id)
    {
        var key = new SensorKey(city, type);
        if (!SensorDictionary.ContainsKey(key))
        {
            SensorDictionary[key] = new List<string>();
        }
        SensorDictionary[key].Add(id);
    }

    public static List<string> GetSensors(Pilot pilot, SensorType type)
    {
        var key = new SensorKey(pilot, type);
        return SensorDictionary.ContainsKey(key) ? SensorDictionary[key] : null;
    }
}

public enum SensorType
{
    air,
    noise,
    temperature
}
