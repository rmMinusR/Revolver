using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class ShotPortalRedirector : ShotPathResolver
{
    protected internal override void ResolvePath()
    {
        Portal portal = null;
        if (shot.lastSeg.hit.collider?.TryGetComponent(out portal) ?? false)
        {
            //Find new output location
            Ray ray = new Ray(shot.lastSeg.end, shot.lastSeg.direction);
            ray.origin    = portal.linkedPortal.transform.localToWorldMatrix.MultiplyPoint ( portal.transform.worldToLocalMatrix.MultiplyPoint (ray.origin   ) );
            ray.direction = portal.linkedPortal.transform.localToWorldMatrix.MultiplyVector( portal.transform.worldToLocalMatrix.MultiplyVector(ray.direction) );

            shot.AppendPathSegment(ray, false);
        }
    }
}
