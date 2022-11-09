using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public sealed class TriggerDriver : MonoBehaviour
{
    [SerializeField] private HammerDriver hammer;
    [SerializeField] private InputActionReference control;

    private void Start()
    {
        control.action.Enable();
        control.action.performed -= OnTriggerPressed;
        control.action.performed += OnTriggerPressed;
    }

    private void OnDestroy()
    {
        control.action.performed -= OnTriggerPressed;
    }

    private void OnTriggerPressed(InputAction.CallbackContext ctx)
    {
        hammer.Release();
    }
}
