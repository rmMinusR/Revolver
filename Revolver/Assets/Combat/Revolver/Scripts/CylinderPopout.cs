using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public sealed class CylinderPopout : MonoBehaviour
{
    private Revolver revolver;

    [SerializeField] private Transform pivot;
    private Quaternion baseRot;

    [SerializeField] FlickGestureRecognizer.Gesture flickOut;
    [SerializeField] FlickGestureRecognizer.Gesture flickIn;

    [SerializeField] [Min(0)] private float popoutAngle = 60;
    public bool IsOut { get; private set; }

    private void Start()
    {
        baseRot = pivot.localRotation;

        revolver = GetComponentInParent<Revolver>();

        flickOut.OnPerformed += PutOut;
        flickIn.OnPerformed += PopIn;

        revolver.flick.gestures.Add(flickOut);
        revolver.flick.gestures.Add(flickIn);
    }

    private void PutOut()
    {
        IsOut = true;
        pivot.localRotation = baseRot * Quaternion.AngleAxis(-popoutAngle, Vector3.forward);
    }

    private void PopIn()
    {
        IsOut = false;
        pivot.localRotation = baseRot;
    }
}
