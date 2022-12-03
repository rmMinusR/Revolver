using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(KeepTargetAtRangeContext))]
public class TargetRangeKeeperBinder : MonoBehaviour
{
    [SerializeField] private AIAttack context;
    private KeepTargetAtRangeContext steering;

    void Start()
    {
        steering = GetComponent<KeepTargetAtRangeContext>();
    }

    void Update()
    {
        if (context.target != null)
        {
            steering.SetTarget(((Component)context.target).transform);
        }
    }
}
