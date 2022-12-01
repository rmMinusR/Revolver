using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public sealed class TriggerColliderSensor : Sensor
{
    private HashSet<Sensable> sensed = new HashSet<Sensable>();
    public override IReadOnlyCollection<Sensable> GetSensed() => sensed;

    public override event Action<Sensable> OnBeganSensing;
    public override event Action<Sensable> OnStoppedSensing;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Sensable s))
        {
            sensed.Add(s);
            OnBeganSensing?.Invoke(s);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out Sensable s))
        {
            sensed.Remove(s);
            OnBeganSensing?.Invoke(s);
        }
    }
}
