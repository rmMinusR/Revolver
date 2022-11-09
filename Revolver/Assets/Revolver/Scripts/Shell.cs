using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class Shell : SparkReciever
{
    [SerializeField] internal ShellSlot slot;
    [SerializeField] private FiredShot onFiredPrefab;

    protected internal override void OnSparked()
    {
        Revolver revolver = GetComponentInParent<Revolver>();

        Instantiate(onFiredPrefab, revolver.bulletOrigin.position, revolver.bulletOrigin.rotation);
        onFiredPrefab = null;
    }
}
