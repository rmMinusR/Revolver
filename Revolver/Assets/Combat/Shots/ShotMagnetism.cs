using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotMagnetism : ShotPathResolver
{
    [SerializeField] [Range(0, 180)] private float angle = 5f;

    protected internal override void ResolvePath()
    {
        if (shot.target == null)
        {
            shot.target = FindTarget(transform.position, transform.forward, angle, out shot.lastSeg.hit).Combat;
        }
    }

    public static ShotMagnetismTarget FindTarget(Vector3 position, Vector3 direction, float angle, out RaycastHit hit)
    {
        ShotMagnetismTarget target = null;
        hit = default;

        float minAngle = float.PositiveInfinity;
        foreach (ShotMagnetismTarget t in FindObjectsOfType<ShotMagnetismTarget>())
        {
            float ang = Vector3.Angle(position - t.transform.position, direction);
            if (target == null || ang < minAngle)
            {
                target = t;
                minAngle = ang;
            }
        }
            
        //Verify it's within magnetism angle range
        if (minAngle > angle) target = null;
        else
        {
            //Redo raycast to make sure we can hit it
            bool hasLineOfSight = Physics.Raycast(position, target.transform.position - position, out hit);
            if (!hasLineOfSight) target = null;
        }

        return target;
    }
}
