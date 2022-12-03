using Combat;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmpoweredShockShot : ShotEffect
{
    [SerializeField] private ChainLightning arcPrefab;

    protected internal override void Hit(ICombatAffector source, ICombatTarget target, RaycastHit hit)
    {
        if (shot.isEmpowered)
        {
            ChainLightning arc = Instantiate(arcPrefab);
            arc.SetSource(source);
            arc.SetStart(hit.point);
            arc.hits.Add(target); //Don't hit twice
        }
    }
}
