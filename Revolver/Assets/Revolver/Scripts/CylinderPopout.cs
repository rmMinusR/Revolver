using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public sealed class CylinderPopout : MonoBehaviour
{
    [SerializeField] private Transform pivot;
    [SerializeField] private Transform centerOfMass;

    [SerializeField] [Min(0)] private float popoutAngle = 60;
    [SerializeField] [Min(0)] private float returnForce = 1;
    [SerializeField] [Min(0)] private float motionInfluence = 1;

    private float currentAngle = 0;
    private float currentVel = 0;

    public bool IsOut => currentAngle > popoutAngle / 2;

    private Vector3 lastPos;
    private Quaternion baseRot;

    private void Start()
    {
        lastPos = transform.position;
        baseRot = pivot.localRotation;
    }

    private void Update()
    {
        //Force from motion
        Vector3 pd = centerOfMass.position - lastPos;
        lastPos = centerOfMass.position;
        currentAngle -= Vector3.Dot(pivot.right, pd * motionInfluence); //TODO make vel instead?

        //Spring away from midpoint
        float distFromMidpoint = currentAngle - popoutAngle/2;
        currentVel += distFromMidpoint * returnForce;

        //Kinematics
        currentAngle += currentVel * Time.deltaTime;

        //Apply limits
        if (currentAngle < 0)
        {
            currentAngle = 0;
            if (currentVel < 0) currentVel = 0;
        }

        if (currentAngle > popoutAngle)
        {
            currentAngle = popoutAngle;
            if (currentVel > 0) currentVel = 0;
        }

        //Visual update
        pivot.localRotation = baseRot * Quaternion.AngleAxis(-currentAngle, Vector3.forward);
    }
}
