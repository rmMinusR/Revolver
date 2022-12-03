using Combat;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(FiredShot))]
public abstract class EmpoweredShotEffect : MonoBehaviour
{
    protected virtual void Start()
    {
        FiredShot fs = GetComponent<FiredShot>();
        if (fs.isEmpowered) Hit(fs.GetSource(), fs.target, fs.hit);
    }

    protected abstract void Hit(ICombatAffector combatAffector, ICombatTarget target, RaycastHit hit);
}
