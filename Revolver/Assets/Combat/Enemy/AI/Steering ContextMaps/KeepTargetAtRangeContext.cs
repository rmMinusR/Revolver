using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeepTargetAtRangeContext : IContextProvider
{
    [SerializeField] [Min(0)] private float idealRange = 4;
    [SerializeField] [Min(0)] private float deadZone = 2;

    [Space]
    [SerializeField] [InspectorReadOnly] private Vector3 staticTarget;
    [SerializeField] [InspectorReadOnly] private Transform liveTarget;

    public void SetTarget(Transform target) => liveTarget = target;
    public void SetTarget(Vector3 target) { staticTarget = target; liveTarget = null; }
    
    protected override void RefreshContextMapValues()
    {
        if (liveTarget) staticTarget = liveTarget.position;

        for (int i = 0; i < entries.Length; ++i)
        {
            Vector3 toAnchor = staticTarget - ContextMap.Host.transform.position;
            float distToAnchor = toAnchor.magnitude;
            float angleToAnchor = Vector3.Angle(toAnchor, entries[i].direction) * Mathf.Deg2Rad;

            float desiredForce = distToAnchor - idealRange;
            desiredForce = Mathf.Sign(desiredForce) * Mathf.Max(0, Mathf.Abs(desiredForce) - deadZone);

            entries[i].value += shapingFunction.Evaluate(angleToAnchor) * desiredForce * weight;
        }
    }
}
