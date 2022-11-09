using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public sealed class CylinderLoader : MonoBehaviour
{
    private Revolver revolver;

    [SerializeField] [Min(0)] private float deadZone = 0.2f;
    [SerializeField] private Vector2 neutralPos = new Vector2(0.5f, 0.5f);

    [Serializable]
    public class Loadable
    {
        public Vector2 position;
        public Shell prefab;
    }
    [SerializeField] private Loadable[] loadables;

    private Loadable GetClosest(Vector2 input)
    {
        float distToNeutral = Vector2.Distance(neutralPos, input);
        if (distToNeutral < deadZone) return null;

        float closestDist = float.MaxValue;
        Loadable closest = null;
        foreach (Loadable i in loadables)
        {
            float iDist = Vector2.Distance(i.position, input);
            if (iDist < closestDist)
            {
                closest = i;
                closestDist = iDist;
            }
        }

        return closestDist < distToNeutral ? closest : null;
    }

    private void Start()
    {
        revolver = GetComponentInParent<Revolver>();
        revolver.loaderControl.action.Enable();
    }

    private Loadable lastSelected = null;
    private void Update()
    {
        Debug.Log(revolver.loaderControl.action.ReadValue<Vector2>());

        if (revolver.cylinderPopout.IsOut)
        {
            Loadable nextSel = GetClosest(revolver.loaderControl.action.ReadValue<Vector2>());
            if (nextSel != null && lastSelected == null)
            {
                revolver.cylinderState.Load(nextSel.prefab);
                revolver.cylinderState.AdvanceCylinder();
            }
            lastSelected = nextSel;
        }
        else lastSelected = null;
    }
}
