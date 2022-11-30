using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StayInAreaContext : IContextProvider
{
    [SerializeField] private Transform anchor;
    [SerializeField] [Min(0)] private float neutralRange = 10;
    
    protected override void RefreshContextMapValues()
    {
        for (int i = 0; i < entries.Length; ++i)
        {
            Vector3 toAnchor = anchor.position - ContextMap.Host.transform.position;
            toAnchor.y = ContextMap.Host.transform.position.y;
            float distToAnchor = toAnchor.magnitude;
            float angleToAnchor = Vector3.Angle(toAnchor, entries[i].direction) * Mathf.Deg2Rad;
            entries[i].value += shapingFunction.Evaluate(angleToAnchor) * Mathf.Max(0, distToAnchor-neutralRange)*weight;
        }
    }
}
