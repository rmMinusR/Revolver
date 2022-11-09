using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public sealed class TriggerDriver : MonoBehaviour
{
    [SerializeField] private HammerDriver hammer;
    private InputAction control;

    private void Start()
    {
        control = GetComponentInParent<Revolver>().triggerControl;

        control.Enable();
        control.performed -= OnTriggerPressed;
        control.performed += OnTriggerPressed;
    }

    private void OnDestroy()
    {
        control.performed -= OnTriggerPressed;
    }

    private void OnTriggerPressed(InputAction.CallbackContext ctx)
    {
        hammer.Release();
    }
}
