using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(FiredShot))]
public class PortalShotEffect : MonoBehaviour
{
    [SerializeField] private Portal portalPrefab;
    [SerializeField] [Min(0)] private float nearDistFromOrigin = 0.5f;

    protected virtual void Start()
    {
        FiredShot fs = GetComponent<FiredShot>();
        
        if (fs.hit.collider)
        {
            Vector3 lookVec = fs.transform.position - fs.hit.point;
            lookVec.y = 0;
            Quaternion look = Quaternion.LookRotation(lookVec);

            Vector3 farPos = fs.hit.point;
            if (Physics.Raycast(farPos, Vector3.down, out RaycastHit farGroundAlign)) farPos = farGroundAlign.point;
            Portal far = Instantiate(portalPrefab, farPos, look);

            Vector3 nearPos = Vector3.MoveTowards(fs.transform.position, fs.hit.point, nearDistFromOrigin);
            if (Physics.Raycast(nearPos, Vector3.down, out RaycastHit nearGroundAlign)) nearPos = nearGroundAlign.point;
            Portal near = Instantiate(portalPrefab, nearPos, look);

            far.linkedPortal = near;
            near.linkedPortal = far;
        }
        
    }


}
