using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class CylinderState : MonoBehaviour
{
    private Revolver revolver;

    private List<ShellSlot> slots;
    public IReadOnlyList<ShellSlot> Slots => slots;
    [SerializeField] private Transform visual;
    [SerializeField] [Min(0)] private float rotateDuration = 0.3f;

    [Space]
    [SerializeField] FlickGestureRecognizer.Gesture flickUnload;
    [SerializeField][Range(0, 1)] private float unloadFailChance = 0.1f;

    private void Start()
    {
        slots = new List<ShellSlot>(GetComponentsInChildren<ShellSlot>());

        revolver = GetComponentInParent<Revolver>();
        flickUnload.OnPerformed += revolver.cylinderState.TryUnload;
        revolver.flick.gestures.Add(flickUnload);
    }

    internal void TryUnload()
    {
        if (revolver.cylinderPopout.IsOut)
        {
            foreach (ShellSlot slot in slots)
            {
                if (UnityEngine.Random.value > unloadFailChance) slot.Unload();
            }
        }

    }

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
