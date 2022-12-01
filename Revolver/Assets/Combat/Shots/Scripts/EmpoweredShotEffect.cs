using Combat;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EmpoweredShotEffect : MonoBehaviour
{
    public abstract void Hit(ICombatAffector combatAffector, ICombatTarget target, RaycastHit hit);
}
