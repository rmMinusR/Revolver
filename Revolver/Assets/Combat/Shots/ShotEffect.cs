using Combat;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ShotCore))]
public abstract class ShotEffect : MonoBehaviour
{
    protected ShotCore shot { get; private set; }

    protected virtual void Start()
    {
        shot = GetComponent<ShotCore>();
    }

    protected internal abstract void Hit(ICombatAffector source, ICombatTarget target, RaycastHit hit);
}
