using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

[RequireComponent(typeof(Portal))]
public sealed class PortalSweepInteract : MonoBehaviour
{
    bool sweeping = false;
    [SerializeField] private AnimationCurve sweepCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
    [SerializeField] [Min(0)] private float sweepTime = 2;

    public void Sweep(ActivateEventArgs args)
    {
        if (!sweeping) StartCoroutine(_Sweep(args));
    }

    private IEnumerator _Sweep(ActivateEventArgs args)
    {
        sweeping = true;

        Vector3 ownAnchor = transform.position;
        Vector3 ownDelta = 2*(args.interactorObject.transform.position-ownAnchor);

        Portal portal = GetComponent<Portal>();

        Vector3 otherAnchor = portal.linkedPortal.transform.position;
        Vector3 otherDelta = portal.linkedPortal.transform.localToWorldMatrix.MultiplyVector( portal.transform.worldToLocalMatrix.MultiplyVector(ownDelta) );

        float startTime = Time.time;
        float endTime = Time.time + sweepTime;
        while (Time.time < endTime)
        {
            float t = sweepCurve.Evaluate(Mathf.InverseLerp(startTime, endTime, Time.time));
            transform.position = ownAnchor + ownDelta * t;
            portal.linkedPortal.transform.position = otherAnchor + otherDelta * t;
            yield return null;
        }

        transform.position = ownAnchor + ownDelta;
        portal.linkedPortal.transform.position = otherAnchor + otherDelta;
        sweeping = false;
    }
}
