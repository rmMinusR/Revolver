using Combat;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ShotCore))]
public class PortalShotEffect : ShotEffect
{
    [SerializeField] private Portal portalPrefab;
    [SerializeField] [Min(0)] private float nearDistFromOrigin = 0.5f;

    protected internal override void Hit(ICombatAffector source, ICombatTarget target, RaycastHit hit)
    {
        if (hit.collider)
        {
            Vector3 lookVec = shot.transform.position - hit.point;
            lookVec.y = 0;
            Quaternion look = Quaternion.LookRotation(lookVec);

            Vector3 farPos = shot.lastSeg.hit.point;
            if (Physics.Raycast(farPos, Vector3.down, out RaycastHit farGroundAlign)) farPos = farGroundAlign.point;
            Portal far = Instantiate(portalPrefab, farPos, look);

            Vector3 nearPos = Vector3.MoveTowards(shot.transform.position, hit.point, nearDistFromOrigin);
            if (Physics.Raycast(nearPos, Vector3.down, out RaycastHit nearGroundAlign)) nearPos = nearGroundAlign.point;
            Portal near = Instantiate(portalPrefab, nearPos, look);

            far.linkedPortal = near;
            near.linkedPortal = far;
        }
    }
}
