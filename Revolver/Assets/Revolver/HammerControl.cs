using UnityEngine;

[RequireComponent(typeof(HingeJoint))]
public sealed class HammerControl : MonoBehaviour
{
    public TouchpadThroughput touchpad;

    [SerializeField] private AnimationCurve pullCurve;

    private HingeJoint pivot;

    [SerializeField] private float lockAngle;
    [SerializeField] private bool locked;

    private void Update()
    {
        JointLimits limits = pivot.limits;
        if (touchpad.IsTouched)
        {
            limits.min = pullCurve.Evaluate(touchpad.Position.y) * limits.max;
            locked &= limits.min > lockAngle;
        }
        else if (locked) limits.min = Mathf.Max(limits.min, lockAngle);
        else limits.min = 0;
        pivot.limits = limits;
    }

    [ContextMenu("Release (manual)")]
    public void Release()
    {
        locked = false;
    }
}
