using UnityEngine;

public sealed class HammerDriver : MonoBehaviour
{
    public TouchpadThroughput touchpad;

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
    [SerializeField] private Transform sparkOrigin;
    [SerializeField] [Min(0)] private float sparkRange = 0.1f;
    [SerializeField] private GameObject sparkFX;

    private void Start()
    {
        touchpad.ActivateControls();
    }

    private void Update()
    {
        //Pulling back
        if (touchpad.IsTouched)
        {
            hammerLoc = Mathf.Clamp01(pullInputCurve.Evaluate(touchpad.Position.y));
            hammerVel = 0;
            locked &= hammerLoc > lockAngle;
        }
        
        //Virtual physics
        hammerLoc += hammerVel * Time.deltaTime;
        hammerVel += -returnForce * Time.deltaTime;

        if (locked && hammerLoc >= lockAngle)
        {
            hammerLoc = lockAngle;
            hammerVel = 0;
        }

        if (hammerLoc <= 0)
        {
            if (Mathf.Abs(hammerVel) > sparkThreshold)
            {
                SparkReciever.SparkNearby(sparkOrigin.position, sparkRange);
                Instantiate(sparkFX, sparkOrigin.position, sparkOrigin.rotation);
            }
            hammerLoc = 0;
            hammerVel = 0;
        }

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
