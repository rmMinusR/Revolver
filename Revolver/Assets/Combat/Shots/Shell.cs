using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.XR.OpenXR.Input;

public sealed class Shell : SparkReciever
{
    public Sprite displayIcon;
    public string displayName;

    [Space]
    [SerializeField] internal ShellSlot slot;
    [SerializeField] private ShotCore onFiredPrefab;
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

            ShotCore shot = Instantiate(onFiredPrefab, revolver.bulletOrigin.position, revolver.bulletOrigin.rotation);
            shot.source = revolver.owner;
            shot.isEmpowered = revolver ? revolver.cylinderState.Slots.Count(i => i.contents != null && !i.contents.hasFired) == 1 : false; //Empower if this is the last shot
            hasFired = true;

            OpenXRInput.SendHapticImpulse(revolver.haptics.action, fireHapticAmp, fireHapticDur);
        }
    }
}
