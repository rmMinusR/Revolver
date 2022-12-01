using Combat;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class EmpoweredFireShot : EmpoweredShotEffect
{
    [SerializeField] [Min(0)] private float dpsTime = 1;
    [SerializeField] [Min(0)] private float dpsTickRate = 2;
    [SerializeField] private List<Damage> dpsTickEffects;
    [SerializeField] private GameObject dpsVfx;

    [Space]
    [SerializeField] [Min(0)] private float explodeRange = 5;
    [SerializeField] private GameObject explosionVfx;

    [Space]
    [SerializeField] private List<Damage> effectsPerTick;

    public override void Hit(ICombatAffector source, ICombatTarget initialTarget, RaycastHit hit)
    {
        //Collect targets
        HashSet<ICombatTarget> affected = new HashSet<ICombatTarget>();
        foreach (Collider c in Physics.OverlapSphere(hit.point, explodeRange)) if (c.TryGetComponent(out ICombatTarget t)) affected.Add(t);

        //Apply
        foreach (ICombatTarget t in affected)
        {
            if (source.GetSentimentTowards(t) == Sentiment.Hostile) RepeatingDamageStatus.Apply(source, t, dpsTime, dpsTickRate, effectsPerTick, dpsVfx);
        }

        //VFX
        if (explosionVfx) Instantiate(explosionVfx, hit.point, Quaternion.identity);
    }
}
