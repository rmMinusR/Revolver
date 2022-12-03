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
            float minAngle = float.PositiveInfinity;
            foreach (ShotMagnetismTarget t in FindObjectsOfType<ShotMagnetismTarget>()) //TODO how to work with breakables?
            {
                float ang = Vector3.Angle(transform.position - t.transform.position, transform.forward);
                if (ang < minAngle)
                {
                    shot.target = t.Combat;
                    minAngle = ang;
                }
            }
            
            //Verify it's within magnetism angle range
            if (minAngle > angle) shot.target = null;
            else
            {
                //Redo raycast to make sure we can hit it
                bool hasLineOfSight = Physics.Raycast(transform.position, ((Component)shot.target).transform.position - transform.position, out shot.lastSeg.hit);
                if (!hasLineOfSight) shot.target = null;
            }
        }
    }
}
