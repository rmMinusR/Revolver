using Combat;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public sealed class ChainLightning : MonoBehaviour, ICombatEffect
{
    [SerializeField] private Transform start;
    [SerializeField] private Transform end;

    [NonSerialized] public List<ICombatTarget> hits = new List<ICombatTarget>();
    [SerializeField] [Min(0)] private float hitInterval = 0.5f;
    [SerializeField] [Min(1)] private int maxHits = 6;

    [SerializeField] private List<Damage> effects;

    private ICombatAffector __source;

    public void SetSource(ICombatAffector source) => __source = source;
    public ICombatAffector GetSource() => __source;
    public void Apply(ICombatTarget target) => CombatAPI.Hit(GetSource(), target, this, effects);

    public void SetStart(Vector3 point) => start.position = point;

    IEnumerator Start()
    {
        end.position = start.position;

        for (int i = 0; i < maxHits; ++i)
        {
            start.position = end.position;

            //Jump to next target, if one is available
            CombatantEntity nextTarget = FindObjectsOfType<CombatantEntity>()
                                            .Where(i => !hits.Contains(i) && GetSource().GetSentimentTowards(i) == Sentiment.Hostile)
                                            .MinBy(i => Vector3.Distance(i.transform.position, end.position));
            if (nextTarget != null)
            {
                Apply(nextTarget);
                hits.Add(nextTarget);
                end.position = nextTarget.transform.position;
            }

            yield return new WaitForSeconds(hitInterval);
        }

        Destroy(gameObject);
    }
}
