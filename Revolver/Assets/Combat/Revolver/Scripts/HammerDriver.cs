using UnityEngine;
using UnityEngine.XR.OpenXR.Input;

public sealed class HammerDriver : MonoBehaviour
{
    private Revolver revolver;
    private TouchpadThroughput touchpad;

    [SerializeField] private AnimationCurve pullInputCurve;
    [SerializeField] private Transform neutralPos;
    [SerializeField] private Transform pulledPos;
    [SerializeField] private Transform hammer;

    [Header("Movement parameters")]
    [SerializeField] private bool drawnBack;
    [SerializeField] private bool tryingToFire;
    [SerializeField] [Range(0, 1)] private float lockAngle = 0.5f;
    [SerializeField] [Min(0)] private float returnForce = 1;
    private float hammerLoc = 0;
    private float hammerVel = 0;

    [Header("Lock feedback")]
    [SerializeField] [Min(0)] private float lockHapticAmp = 1;
    [SerializeField] [Min(0)] private float lockHapticDur = 1;

    [Header("Impact feedback")]
    [SerializeField] private SparkEmitter sparkEmitter;

    private void Start()
    {
        revolver = GetComponentInParent<Revolver>();
        touchpad = revolver.touchpadControl;

        touchpad.ActivateControls();
    }

    private void Update()
    {
        //Pulling back
        if (touchpad.IsTouched)
        {
            hammerLoc = Mathf.Clamp01(pullInputCurve.Evaluate(touchpad.Position.y));
            hammerVel = 0;
            if (!drawnBack && hammerLoc > lockAngle)
            {
                drawnBack = true;
                tryingToFire = false;
                if (!revolver.cylinderPopout.IsOut) revolver.cylinderState.AdvanceCylinder();
                OpenXRInput.SendHapticImpulse(revolver.haptics.action, lockHapticAmp, lockHapticDur);
            }
        }
        
        //Virtual physics
        hammerLoc += hammerVel * Time.deltaTime;
        hammerVel += -returnForce * Time.deltaTime;

        //If locked, hold angle
        if (drawnBack && !tryingToFire && hammerLoc <= lockAngle)
        {
            hammerLoc = lockAngle;
            hammerVel = 0;
        }

        //If hitting lower bound, hold angle. If hitting fast enough, ignite.
        if (hammerLoc <= 0)
        {
            if (drawnBack && tryingToFire) sparkEmitter.Fire();
            hammerLoc = 0;
            hammerVel = 0;
            tryingToFire = false;
            drawnBack = false;
        }

        //Update visuals
        hammer.SetPositionAndRotation(
            Vector3.Lerp(neutralPos.position, pulledPos.position, hammerLoc),
            Quaternion.Slerp(neutralPos.rotation, pulledPos.rotation, hammerLoc)
        );
    }

    [ContextMenu("Release (manual)")]
    public void Release()
    {
        tryingToFire = true;
    }
}
