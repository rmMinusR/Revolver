using UnityEngine;

public sealed class HammerDriver : MonoBehaviour
{
    private TouchpadThroughput touchpad;

    [SerializeField] private AnimationCurve pullInputCurve;
    [SerializeField] private Transform neutralPos;
    [SerializeField] private Transform pulledPos;
    [SerializeField] private Transform hammer;

    [Header("Movement parameters")]
    [SerializeField] private bool locked;
    [SerializeField] [Range(0, 1)] private float lockAngle = 0.5f;
    [SerializeField] [Min(0)] private float returnForce = 1;
    private float hammerLoc = 0;
    private float hammerVel = 0;

    [Header("Sparks")]
    [SerializeField] [Min(0)] private float sparkThreshold = 0.3f;
    [SerializeField] private SparkEmitter sparkEmitter;

    private void Start()
    {
        touchpad = GetComponentInParent<Revolver>().touchpadControl;

        touchpad.ActivateControls();
    }

    private void Update()
    {
        //Pulling back
        if (touchpad.IsTouched)
        {
            hammerLoc = Mathf.Clamp01(pullInputCurve.Evaluate(touchpad.Position.y));
            hammerVel = 0;
            locked |= hammerLoc > lockAngle;
        }
        
        //Virtual physics
        hammerLoc += hammerVel * Time.deltaTime;
        hammerVel += -returnForce * Time.deltaTime;

        //If locked, hold angle
        if (locked && hammerLoc <= lockAngle)
        {
            hammerLoc = lockAngle;
            hammerVel = 0;
        }

        //If hitting lower bound, hold angle. If hitting fast enough, ignite.
        if (hammerLoc <= 0)
        {
            if (Mathf.Abs(hammerVel) > sparkThreshold) sparkEmitter.Fire();
            hammerLoc = 0;
            hammerVel = 0;
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
        locked = false;
    }
}
