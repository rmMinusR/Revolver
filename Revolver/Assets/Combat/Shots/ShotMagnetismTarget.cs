using Combat;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ICombatTarget))]
public sealed class ShotMagnetismTarget : MonoBehaviour
{
    [SerializeField] [Range(0, 180)] private float additionalMagnetismAngle;
    internal float AdditionalMagnetismAngle => additionalMagnetismAngle;

    private static HashSet<ShotMagnetismTarget> __Instances = new HashSet<ShotMagnetismTarget>();
    public static IReadOnlyCollection<ShotMagnetismTarget> Instances => __Instances;

    public ICombatTarget Combat => GetComponent<ICombatTarget>();

    private void OnEnable()
    {
        __Instances.Add(this);
    }

    private void OnDisable()
    {
        __Instances.Remove(this);
    }
}