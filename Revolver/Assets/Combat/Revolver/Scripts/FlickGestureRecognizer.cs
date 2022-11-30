using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public sealed class FlickGestureRecognizer : MonoBehaviour
{
    [SerializeField] [Min(1)] private int historyLength = 5;
    private List<Vector3> motionHistory = new List<Vector3>(); //0 is oldest, len-1 is most recent

    private Vector3 lastPos;

    public List<Gesture> gestures = new List<Gesture>();
    [SerializeField] [Min(0)] private float cooldown = 0.3f;
    private float cooldownRemaining = 0;

    private void Start()
    {
        lastPos = transform.position;
    }

    private void Update()
    {
        //Read input
        Vector3 instantaneousMotionGlobal = transform.position - lastPos;
        lastPos = transform.position;
        motionHistory.Add(transform.worldToLocalMatrix.MultiplyVector(instantaneousMotionGlobal));

        while (motionHistory.Count > historyLength) motionHistory.RemoveAt(0);

        //Attempt to recognize gestures
        if (cooldownRemaining > 0) cooldownRemaining -= Time.deltaTime;
        else
        {
            (Gesture, float) toPerform = gestures.Select(g => (g, g.Eval(motionHistory))).MinBy(i => i.Item2);
            if (toPerform.Item1.IsValid(motionHistory))
            {
                cooldownRemaining = cooldown;
                toPerform.Item1.Perform();
            }
        }
    }

    [Serializable]
    public class Gesture
    {
        [SerializeField] private Vector3 linear;
        [SerializeField] [Min(0)] private float deadZone = 0.05f;

        public event Action OnPerformed;
        public void Perform() => OnPerformed?.Invoke();

        public bool IsValid(List<Vector3> inputs) => inputs.All(i => i.magnitude > deadZone);

        public float Eval(List<Vector3> inputs)
        {
            float val = 0;

            Vector3 linear = this.linear.normalized;
            for (int i = 0; i < inputs.Count; ++i) inputs[i].Normalize();

            for (int i = 0; i < inputs.Count; ++i)
            {
                Vector3 effectiveInput = inputs[i];
                if (effectiveInput.sqrMagnitude > linear.sqrMagnitude) effectiveInput = effectiveInput.normalized * linear.magnitude;
                
                if (effectiveInput.magnitude > deadZone) val += Vector3.Distance(effectiveInput, linear) / 180;
            }

            return val;
        }
    }
}
