using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class CylinderState : MonoBehaviour
{
    private List<ShellSlot> slots;

    private void Start()
    {
        slots = new List<ShellSlot>(GetComponentsInChildren<ShellSlot>());
    }

    [Space]
    [SerializeField] internal Transform bulletOrigin;

    [ContextMenu("Fire")]
    public void Fire()
    {
        if (slots[0].contents != null) slots[0].contents.OnSparked();

        AdvanceCylinder();
    }

    public void Load(Shell bulletPrefab)
    {
        if (slots[0].contents == null) slots[0].Place(bulletPrefab);

        AdvanceCylinder();
    }

    [ContextMenu("Advance Cylinder")]
    public void AdvanceCylinder()
    {
        ShellSlot s = slots[0];
        slots.RemoveAt(0);
        slots.Add(s);
    }
}
