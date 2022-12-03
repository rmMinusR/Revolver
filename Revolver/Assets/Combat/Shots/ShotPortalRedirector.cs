using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class ShotPortalRedirector : ShotPathResolver
{
    protected internal override void ResolvePath()
    {
        if (shot.lastSeg.hit.collider.TryGetComponent(out Portal portal))
        {
            //Find new output location
            Ray ray = new Ray(shot.lastSeg.end, shot.lastSeg.direction);
            ray.origin = portal.linkedPortal.transform.localToWorldMatrix.MultiplyPoint ( portal.transform.worldToLocalMatrix.MultiplyPoint (ray.origin   ) );
            ray.origin = portal.linkedPortal.transform.localToWorldMatrix.MultiplyVector( portal.transform.worldToLocalMatrix.MultiplyVector(ray.direction) );

            shot.AppendPathSegment(ray, true);
        }
    }
}
