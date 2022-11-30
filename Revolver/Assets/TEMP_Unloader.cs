using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TEMP_Unloader : MonoBehaviour
{
    [SerializeField] private InputActionReference input;

    private void Start()
    {
        input.action.Enable();
        input.action.performed += Action_performed;
    }

    private void Action_performed(InputAction.CallbackContext ctx)
    {
        GetComponent<CylinderState>().TryUnload();
    }
}
