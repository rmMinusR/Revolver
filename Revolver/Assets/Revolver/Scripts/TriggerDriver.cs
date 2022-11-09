using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public sealed class TriggerDriver : MonoBehaviour
{
    private Revolver revolver;

    private void Start()
    {
        revolver = GetComponentInParent<Revolver>();
        revolver.triggerControl.action.Enable();
    }

    private void Update()
    {
        if (revolver.triggerControl.action.ReadValue<float>() > 0.5f) revolver.hammer.Release();
    }
}
