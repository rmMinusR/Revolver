using Combat;
using Events;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageAmpStatus : ScopedListener//, ICombatEffect
{
    private float timeRemaining;
    private float incomingDamageMultiplier = 2;
    private ICombatTarget target;

    [EventHandler(Priority.Highest)]
    private void AmpIncomingDamage(HitEvent ev)
    {
        if (ev.target == target)
        {
            for (int i = 0; i < ev.effects.Count; ++i)
            {
                Damage d = ev.effects[i];
                d.damageAmount *= incomingDamageMultiplier;
                ev.effects[i] = d;
            }
        }
    }

    public static DamageAmpStatus Apply(ICombatAffector source, ICombatTarget target, float time, float damageMultiplier, GameObject vfx)
    {
        GameObject go = new GameObject(nameof(RepeatingDamageStatus));
        if (target is Component c)
        {
            go.transform.parent = c.transform;
            go.transform.localPosition = Vector3.zero;
            if (vfx != null) Instantiate(vfx, go.transform).transform.localPosition = Vector3.zero;
        }

        DamageAmpStatus s = go.AddComponent<DamageAmpStatus>();
        s.target = target;
        s.timeRemaining = time;
        s.incomingDamageMultiplier = damageMultiplier;

        return s;
    }
}
