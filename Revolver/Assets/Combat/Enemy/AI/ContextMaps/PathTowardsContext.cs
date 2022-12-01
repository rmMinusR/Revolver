using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class PathTowardsContext : IContextProvider
{
    [SerializeField] [Min(0)] private float deadZone = 2;

    [Space]
    [SerializeField] private bool reachedTarget = true;
    [SerializeField] private Vector3 currentTarget;

    public void SetTarget(Vector3 target)
    {
        reachedTarget = false;
        currentTarget = target;
    }

    protected override void RefreshContextMapValues()
    {
        if (!reachedTarget)
        {
            currentTarget.y = ContextMap.Host.transform.position.y;

            for (int i = 0; i < entries.Length; ++i)
            {
                Vector3 toAnchor = currentTarget - ContextMap.Host.transform.position;
                float distToAnchor = toAnchor.magnitude;
                float angleToAnchor = Vector3.Angle(toAnchor, entries[i].direction) * Mathf.Deg2Rad;
                entries[i].value += shapingFunction.Evaluate(angleToAnchor) * Mathf.Max(0, distToAnchor-deadZone)*weight;
            }
        }
    }

    private void OnDrawGizmos()
    {
        if (!Application.isPlaying) return;

        Gizmos.color = !reachedTarget ? Color.green : Color.gray;
        Gizmos.DrawLine(transform.position, currentTarget);
    }
}
