using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Represents a sensor that must check periodically
/// </summary>
public abstract class ActiveQueryingSensor : Sensor
{
    [SerializeField] [Min(0)] protected float range = 5;
    [SerializeField] [Min(0)] protected float updateFrequency = 0.1f;

    private Coroutine updateWorker;

    protected virtual void OnEnable()
    {
        updateWorker = StartCoroutine(_UpdateWorker());
    }

    protected virtual void OnDisable()
    {
        StopCoroutine(updateWorker);

        sensed.Clear();
        ComputeDiffs();
    }

    private IEnumerator _UpdateWorker()
    {
        while (true)
        {
            UpdateSensor();
            ComputeDiffs();
            yield return new WaitForSeconds(updateFrequency);
        }
    }

    protected HashSet<Sensable> sensed = new HashSet<Sensable>();
    public override IReadOnlyCollection<Sensable> GetSensed() => sensed;
    protected abstract void UpdateSensor();

    private HashSet<Sensable> prevSensed = new HashSet<Sensable>();
    public override event Action<Sensable> OnBeganSensing;
    public override event Action<Sensable> OnStoppedSensing;
    private void ComputeDiffs()
    {
        if (OnStoppedSensing != null || OnBeganSensing != null)
        {
            //Compute what was added and removed
            HashSet<Sensable> added = new HashSet<Sensable>(sensed);
            added.ExceptWith(prevSensed);

            HashSet<Sensable> removed = new HashSet<Sensable>(prevSensed);
            removed.ExceptWith(sensed);

            //Dispatch events
            if (OnStoppedSensing != null) foreach(Sensable i in removed) OnStoppedSensing(i);
            if (OnBeganSensing   != null) foreach(Sensable i in added  ) OnBeganSensing  (i);
        }

        //Update prevSensed
        prevSensed.Clear();
        prevSensed.UnionWith(sensed);
    }
}
