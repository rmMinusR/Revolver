using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotMagnetism : ShotPathResolver
{
    [SerializeField] [Range(0, 180)] private float baseAngle = 5f;

    protected internal override void ResolvePath()
    {
        if (shot.target == null)
        {
            RaycastHit tmpHit;
            shot.target = FindTarget(transform.position, transform.forward, baseAngle, out tmpHit)?.Combat;
            if (shot.target != null) shot.lastSeg.hit = tmpHit;
        }
    }

    public static ShotMagnetismTarget FindTarget(Vector3 position, Vector3 direction, float magnetismAngle, out RaycastHit hit)
    {
        ShotMagnetismTarget target = null;
        hit = default;

        float minAngle = float.PositiveInfinity;
        foreach (ShotMagnetismTarget t in FindObjectsOfType<ShotMagnetismTarget>())
        {
            float ang = Vector3.Angle(position - t.transform.position, direction);
            if (ang < magnetismAngle+t.AdditionalMagnetismAngle) //Verify it's within magnetism angle range
            {
                if (target == null || ang < minAngle) //Verify it's the closest match
                {
                    target = t;
                    minAngle = ang;
                }
            }

        }

        if (target != null)
        {
            //Redo raycast to make sure we can hit it
            bool hasLineOfSight = Physics.Raycast(position, target.transform.position - position, out hit, 100, Physics.DefaultRaycastLayers, QueryTriggerInteraction.Ignore);
            if (!hasLineOfSight) target = null;
        }

        return target;
    }
}
