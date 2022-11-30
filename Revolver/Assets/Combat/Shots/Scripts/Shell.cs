using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.OpenXR.Input;

public sealed class Shell : SparkReciever
{
    [SerializeField] internal ShellSlot slot;
    [SerializeField] private FiredShot onFiredPrefab;
    [Min(0)] public float casingPersistTime = 3;

    public bool hasFired = false;

    [Space]
    [SerializeField] [Min(0)] private float fireHapticAmp = 1;
    [SerializeField] [Min(0)] private float fireHapticDur = 1;

    protected internal override void OnSparked()
    {
        if (!hasFired)
        {
            Revolver revolver = GetComponentInParent<Revolver>();

            FiredShot shot = Instantiate(onFiredPrefab, revolver.bulletOrigin.position, revolver.bulletOrigin.rotation);
            shot.SetSource(revolver.owner);
            hasFired = true;

            OpenXRInput.SendHapticImpulse(revolver.haptics.action, fireHapticAmp, fireHapticDur);
        }
    }
}
