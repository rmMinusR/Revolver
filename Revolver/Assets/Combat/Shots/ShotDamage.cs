using Combat;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ShotCore))]
public class ShotDamage : ShotEffect, ICombatEffect
{
    protected internal override void Hit(ICombatAffector source, ICombatTarget target, RaycastHit hit)
    {
        SetSource(source);
        if (target != null && source.GetSentimentTowards(target) != Sentiment.Friendly) Apply(target);
    }

    [SerializeField] private List<Damage> effects;
    
    private ICombatAffector __source;
    public void SetSource(ICombatAffector source) => __source = source;
    public ICombatAffector GetSource() => __source;
    public void Apply(ICombatTarget target) => CombatAPI.Hit(GetSource(), target, this, effects);
}
