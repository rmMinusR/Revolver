using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public sealed class Revolver : MonoBehaviour
{
    [Header("Components")]
    public HammerDriver hammer;
    public TriggerDriver trigger;
    public CylinderState cylinderState;
    public CylinderPopout cylinderPopout;
    public CylinderLoader cylinderLoader;
    public Transform bulletOrigin;

    [Header("Controls")]
    public TouchpadThroughput touchpadControl;
    public InputActionReference triggerControl;
    public InputActionReference loaderControl;
    public InputActionReference haptics;
}