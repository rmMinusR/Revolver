using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotRicochetRedirector : ShotPathResolver
{
    [SerializeField] [Range(0, 180)] private float magnetismAngle = 90f;

    private bool isProcessing = false;

    protected internal override void ResolvePath()
    {
        if (!isProcessing && shot.lastSeg.hit.collider.GetComponentInParent<Ricochet>())
        {
            //Find new output location
            Ray ray = new Ray(shot.lastSeg.end, Vector3.Reflect(shot.lastSeg.direction, shot.lastSeg.hit.normal));

            //Try to magnetize
            ShotMagnetismTarget newTarget = ShotMagnetism.FindTarget(ray.origin, ray.direction, magnetismAngle, out _);
            if (newTarget) ray.direction = (newTarget.transform.position - ray.origin).normalized;

            //Process
            isProcessing = true;
            shot.AppendPathSegment(ray, true);
            isProcessing = false;
        }
    }
}
