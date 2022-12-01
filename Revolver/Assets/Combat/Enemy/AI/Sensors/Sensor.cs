using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class Sensor : MonoBehaviour
{
    public abstract IReadOnlyCollection<Sensable> GetSensed();
    public abstract event Action<Sensable> OnBeganSensing;
    public abstract event Action<Sensable> OnStoppedSensing;
}
