using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class CylinderState : MonoBehaviour
{
    [Serializable]
    public class Slot
    {
        public Transform visualRoot;
        public Bullet loaded;
    }

    [SerializeField] private List<Slot> slots;

    [Space]
    [SerializeField] private Transform bulletOrigin;

    [ContextMenu("Fire")]
    public void Fire()
    {
        if (slots[0].loaded != null)
        {
            slots[0].loaded.Fire(bulletOrigin);
            Destroy(slots[0].loaded.gameObject);
            slots[0].loaded = null;
        }

        AdvanceCylinder();
    }

    public void Load(Bullet bulletPrefab)
    {
        if (slots[0].loaded == null) slots[0].loaded = Instantiate(bulletPrefab, slots[0].visualRoot);

        AdvanceCylinder();
    }

    [ContextMenu("Advance Cylinder")]
    public void AdvanceCylinder()
    {
        Slot s = slots[0];
        slots.RemoveAt(0);
        slots.Add(s);
    }
}
