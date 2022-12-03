using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PathTowardsContext))]
public class TargetedWander : MonoBehaviour
{
    private PathTowardsContext pather;
    
    [SerializeField] private Transform anchor;
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

    private void GenRandomTarget() => pather.SetTarget( (anchor != null ? anchor.position : transform.position) + targetRandomRange*Random.insideUnitSphere );

    private void OnEnable()
    {
        pather = GetComponent<PathTowardsContext>();
        StartCoroutine(TargetRefresher());
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }
}
