using Combat;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmpoweredAcidShot : EmpoweredShotEffect
{
    [SerializeField] [Min(0)] private float effectTime = 1;
    [SerializeField] [Min(0)] private float damageMultiplier = 2;
    [SerializeField] private GameObject effectVfx;

    protected override void Hit(ICombatAffector source, ICombatTarget target, RaycastHit hit)
    {
        DamageAmpStatus.Apply(source, target, effectTime, damageMultiplier, effectVfx);
    }
}
