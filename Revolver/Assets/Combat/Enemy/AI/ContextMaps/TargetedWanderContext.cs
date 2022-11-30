using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetedWanderContext : IContextProvider
{
    [SerializeField] private Transform anchor;
    [SerializeField] [Min(0)] private float deadZone = 2;
    
    protected override void RefreshContextMapValues()
    {
        if (currentTarget.HasValue)
        {
            for (int i = 0; i < entries.Length; ++i)
            {
                Vector3 toAnchor = currentTarget.Value - ContextMap.Host.transform.position;
                toAnchor.y = ContextMap.Host.transform.position.y;
                float distToAnchor = toAnchor.magnitude;
                float angleToAnchor = Vector3.Angle(toAnchor, entries[i].direction) * Mathf.Deg2Rad;
                entries[i].value += shapingFunction.Evaluate(angleToAnchor) * Mathf.Max(0, distToAnchor-deadZone)*weight;
            }
        }
    }

    Vector3? currentTarget;
    [SerializeField] [Min(0)] private float targetRandomRange = 8;
    [SerializeField] [Min(0)] private float targetRefreshTimeAvg = 10;
    [SerializeField] [Min(0)] private float targetRefreshTimeVariance = 3;
    IEnumerator TargetRefresher()
    {
        GenRandomTarget();
        yield return new WaitForSeconds(Random.value * targetRefreshTimeAvg);

        while (true)
        {
            GenRandomTarget();
            yield return new WaitForSeconds(targetRefreshTimeAvg + targetRefreshTimeVariance*Random.Range(-1, 1));
        }
    }
    private void GenRandomTarget() => currentTarget = (anchor != null ? anchor.position : transform.position) + targetRandomRange*Random.insideUnitSphere;

    private void OnEnable()
    {
        StartCoroutine(TargetRefresher());
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    private void OnDrawGizmos()
    {
        if (!Application.isPlaying) return;

        Gizmos.color = Color.green;
        if (currentTarget.HasValue) Gizmos.DrawLine(transform.position, currentTarget.Value);
    }
}
