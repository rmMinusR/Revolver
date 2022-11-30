using Combat;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(FlickGestureRecognizer))]
public sealed class Revolver : MonoBehaviour
{
    public CombatantEntity owner;

    [Header("Components")]
    public FlickGestureRecognizer flick;
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