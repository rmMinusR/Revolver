using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SightSensor : ActiveQueryingSensor
{
    [SerializeField] [Range(0, 360)] private float fov = 75;
    [SerializeField] private LayerMask obstructingLayers = Physics.DefaultRaycastLayers; //Default to all but Ignore Raycast

    private Collider[] _withinRange = new Collider[64];
    private int _withinRangeValidCount = 0;
    protected override void UpdateSensor()
    {
        sensed.Clear();

        _withinRangeValidCount = Physics.OverlapSphereNonAlloc(transform.position, range, _withinRange);
        for (int i = 0; i < _withinRangeValidCount; ++i)
        {
            if (_withinRange[i].TryGetComponent(out Sensable s) && CanSee(s))
            {
                sensed.Add(s);
            }
        }
    }

    private bool CanSee(Sensable s)
    {
        Vector3 d = s.transform.position-transform.position;
        Ray ray = new Ray(transform.position, d);
        return Vector3.Angle(transform.forward, ray.direction) <= fov/2    //Is it within FOV cone?
            && Physics.Raycast(ray, out RaycastHit hit, d.magnitude, obstructingLayers)
            && hit.transform == s.transform;                               //Do we have line of sight?
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Vector2 flat = new Vector2(Mathf.Cos(fov/2*Mathf.Deg2Rad), Mathf.Sin(fov/2*Mathf.Deg2Rad));
        Vector3 corner1 = transform.forward*flat.x + transform.right * flat.y;
        Vector3 corner2 = transform.forward*flat.x + transform.up    * flat.y;
        Vector3 corner3 = transform.forward*flat.x - transform.right * flat.y;
        Vector3 corner4 = transform.forward*flat.x - transform.up    * flat.y;

        //Draw cone
        Gizmos.DrawLine(transform.position, transform.position + corner1*range);
        Gizmos.DrawLine(transform.position, transform.position + corner2*range);
        Gizmos.DrawLine(transform.position, transform.position + corner3*range);
        Gizmos.DrawLine(transform.position, transform.position + corner4*range);

        //Connect cone to center
        Gizmos.DrawLine(transform.position + transform.forward*range, transform.position + corner1*range);
        Gizmos.DrawLine(transform.position + transform.forward*range, transform.position + corner2*range);
        Gizmos.DrawLine(transform.position + transform.forward*range, transform.position + corner3*range);
        Gizmos.DrawLine(transform.position + transform.forward*range, transform.position + corner4*range);
    }
}
