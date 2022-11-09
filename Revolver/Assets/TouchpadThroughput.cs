using System;
using UnityEngine;
using UnityEngine.InputSystem;

[Serializable]
public struct TouchpadThroughput
{
    [SerializeField] private InputActionReference touchedControl;
    [SerializeField] private InputActionReference posControl;

    public bool IsTouched => touchedControl.action.ReadValue<float>() > 0.5f;
    public Vector2 Position => posControl.action.ReadValue<Vector2>();

    public void ActivateControls()
    {
        touchedControl.action.Enable();
        posControl.action.Enable();
    }
}
