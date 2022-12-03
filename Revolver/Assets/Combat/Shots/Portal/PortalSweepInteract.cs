using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

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

        Vector3 start = transform.position;
        Vector3 end = start + 2*(args.interactorObject.transform.position-transform.position);

        float startTime = Time.time;
        float endTime = Time.time + sweepTime;
        while (Time.time < endTime)
        {
            float t = sweepCurve.Evaluate(Mathf.InverseLerp(startTime, endTime, Time.time));
            transform.position = Vector3.Lerp(start, end, t);
            yield return null;
        }

        transform.position = end;
        sweeping = false;
    }
}
