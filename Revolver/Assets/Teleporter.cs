using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

public sealed class Teleporter : MonoBehaviour
{
    [SerializeField] private Transform moveTarget;
    [SerializeField] private Transform raycastOrigin;

    [SerializeField] private string teleportBinding = "Teleport";
    private InputAction teleportControl;

    private void Start()
    {
        teleportControl.performed += DoTeleport;
    }

    private void DoTeleport(InputAction.CallbackContext ctx)
    {
        if (NavMesh.Raycast(raycastOrigin.position, raycastOrigin.forward, out NavMeshHit hit, ~0))
        {
            moveTarget.position = hit.position;
        }
    }
}
