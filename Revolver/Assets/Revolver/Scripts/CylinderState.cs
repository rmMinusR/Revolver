using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class CylinderState : MonoBehaviour
{
    private List<ShellSlot> slots;
    [SerializeField] private Transform visual;
    [SerializeField] [Min(0)] private float rotateDuration = 0.3f;

    private void Start()
    {
        slots = new List<ShellSlot>(GetComponentsInChildren<ShellSlot>());
    }

    [Space]
    [SerializeField] internal Transform bulletOrigin;

    public void Load(Shell bulletPrefab)
    {
        if (slots[0].contents == null) slots[0].Place(bulletPrefab);
    }

    [ContextMenu("Advance Cylinder")]
    public void AdvanceCylinder()
    {
        //Rotate indices
        ShellSlot s = slots[0];
        slots.RemoveAt(0);
        slots.Add(s);

        //Rotate visually
        StartCoroutine(RotateVisuals(360 / slots.Count, rotateDuration));
    }

    IEnumerator RotateVisuals(float totalRotation, float duration)
    {
        float remainingRotation = totalRotation;
        while (remainingRotation > 0)
        {
            float toRotate = totalRotation * Time.deltaTime/duration;
            toRotate = Mathf.Min(remainingRotation, toRotate);
            remainingRotation -= toRotate;
            visual.Rotate(0, 0, toRotate, Space.Self);
            yield return null;
        }
    }
}
