using Combat;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ShotCore))]
public abstract class ShotEffect : MonoBehaviour
{
    private ShotCore __shot;
    protected ShotCore shot => __shot != null ? __shot : (__shot = GetComponent<ShotCore>());

    protected internal abstract void Hit(ICombatAffector source, ICombatTarget target, RaycastHit hit);
}