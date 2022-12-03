using Combat;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmpoweredAcidShot : ShotEffect
{
    [SerializeField] [Min(0)] private float effectTime = 1;
    [SerializeField] [Min(0)] private float damageMultiplier = 2;
    [SerializeField] private GameObject effectVfx;

    protected internal override void Hit(ICombatAffector source, ICombatTarget target, RaycastHit hit)
    {
        if (shot.isEmpowered) DamageAmpStatus.Apply(source, target, effectTime, damageMultiplier, effectVfx);
    }
}
