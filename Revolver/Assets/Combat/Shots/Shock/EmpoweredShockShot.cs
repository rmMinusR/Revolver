using Combat;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmpoweredShockShot : EmpoweredShotEffect
{
    [SerializeField] private ChainLightning arcPrefab;

    protected override void Hit(ICombatAffector combatAffector, ICombatTarget target, RaycastHit hit)
    {
        ChainLightning arc = Instantiate(arcPrefab);
        arc.SetSource(combatAffector);
        arc.SetStart(hit.point);
        arc.hits.Add(target); //Don't hit twice
    }
}
