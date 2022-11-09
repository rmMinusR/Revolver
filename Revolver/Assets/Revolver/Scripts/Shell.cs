using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class Shell : SparkReciever
{
    [SerializeField] internal ShellSlot slot;
    [SerializeField] private FiredShot onFiredPrefab;

    protected internal override void OnSparked()
    {
        CylinderState manager = GetComponentInParent<CylinderState>();

        Instantiate(onFiredPrefab, manager.bulletOrigin.position, manager.bulletOrigin.rotation);
        onFiredPrefab = null;
    }
}
