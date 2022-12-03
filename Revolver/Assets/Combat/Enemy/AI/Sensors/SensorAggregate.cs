using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles multiple overlapping sensors
/// </summary>
public sealed class SensorAggregate : Sensor
{
    [SerializeField] private List<Sensor> sensors;

    private Dictionary<Sensable, int> sensed = new Dictionary<Sensable, int>();
    public override IReadOnlyCollection<Sensable> GetSensed() => sensed.Keys;

    public override event Action<Sensable> OnBeganSensing;
    public override event Action<Sensable> OnStoppedSensing;

    private void BeginSensingProxy(Sensable s)
    {
        int count;
        if (!sensed.TryGetValue(s, out count))
        {
            count = 0;
            OnBeganSensing?.Invoke(s);
        }
        sensed[s] = count;
    }

    private void StoppedSensingProxy(Sensable s)
    {
        if (sensed.TryGetValue(s, out int newCount))
        {
            --newCount;

            if (newCount > 0) sensed[s] = newCount;
            else
            {
                sensed.Remove(s);
                OnStoppedSensing?.Invoke(s);
            }
        }
    }

    private void OnEnable()
    {
        foreach (Sensor s in sensors)
        {
            s.OnBeganSensing += BeginSensingProxy;
            s.OnStoppedSensing += StoppedSensingProxy;
        }
    }

    private void OnDisable()
    {
        foreach (Sensor s in sensors)
        {
            s.OnBeganSensing -= BeginSensingProxy;
            s.OnStoppedSensing -= StoppedSensingProxy;
        }
    }
}
