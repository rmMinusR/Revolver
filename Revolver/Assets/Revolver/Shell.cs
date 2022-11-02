using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class Shell : MonoBehaviour
{
    [SerializeField] internal ShellSlot slot;
    [SerializeField] private FiredShot onFiredPrefab;

    internal void Ignite()
    {
        CylinderState manager = GetComponentInParent<CylinderState>();

        Instantiate(onFiredPrefab, manager.bulletOrigin.position, manager.bulletOrigin.rotation);
        onFiredPrefab = null;
    }
}
