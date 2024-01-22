using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

namespace GoNatureAR
{
    [Serializable]
    public enum Pilot
    {
        chania,
        dundalk,
        castelfranco_veneto,
        leuven,
        gzira,
        skelleftea,
        novo_mesto
    }

    [Serializable]
    public enum SensorType
    {
        air,
        thermal,
        noise
    }
}
