using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Sensors", menuName = "Sensors/SensorInfo")]
public class SensorInfoSO : ScriptableObject
{
    public SensorInfo[] sensorsInfo;
    
    public Dictionary<SensorKey, string> keyValuePairs= new Dictionary<SensorKey, string>();
}

[Serializable]
public class SensorInfo
{
    public Pilot pilotName;
    public SensorType sensorType;
    public string sensorID;
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

public enum SensorType
{
    air,
    noise,
    temperature
}
