using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public sealed class FlickGestureRecognizer : MonoBehaviour
{
    [SerializeField] [Min(1)] private int historyLength = 5;

    [Serializable]
    public struct Frame
    {
        public float time;

        //Both of these are in RELATIVE space
        public Vector3 instantVel;
        public Vector3 instantAccel;
    }
    private List<Frame> motionHistory = new List<Frame>(); //0 is oldest, len-1 is most recent

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
        Frame? prevFrame = motionHistory.Count != 0 ? motionHistory[motionHistory.Count-1] : null; //Helper

        //Read input
        Frame frame = new Frame();
        frame.time = Time.unscaledTime;
        frame.instantVel = transform.worldToLocalMatrix.MultiplyVector( transform.position - lastPos );
        if (prevFrame.HasValue) frame.instantAccel = frame.instantVel - prevFrame.Value.instantVel;

        //Normalize for delta time
        if (prevFrame.HasValue)
        {
            float dtNrm = frame.time-prevFrame.Value.time;
            frame.instantVel /= dtNrm;
            frame.instantAccel /= dtNrm;
        }

        //Upkeep
        motionHistory.Add(frame);
        while (motionHistory.Count > historyLength) motionHistory.RemoveAt(0);
        lastPos = transform.position;

        //Attempt to recognize gestures
        if (cooldownRemaining > 0) cooldownRemaining -= Time.deltaTime;
        else
        {
            (Gesture, float) toPerform = gestures.Where(g => g.IsValid(motionHistory)).Select(g => (g, g.Eval(motionHistory))).MinBy(i => i.Item2);
            if (toPerform.Item1 != null && toPerform.Item1.IsValid(motionHistory))
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
        //[SerializeField] [Min(0)] private float accelDeadZone = 0.05f;
        [SerializeField] [Min(0)] private float deadZonePenalty = 1;
        [SerializeField] [Min(0)] private float minAccel = 5f;

        public event Action OnPerformed;
        public void Perform() => OnPerformed?.Invoke();

        public bool IsValid(List<Frame> inputs) => inputs.All(i => i.instantVel.magnitude > deadZone) && -inputs.Average(i => Vector3.Dot(linear, i.instantAccel)) > minAccel;

        public float Eval(List<Frame> inputs)
        {
            return inputs.Sum(
                i => i.instantVel.magnitude > deadZone
                    ? Vector3.Angle(linear, i.instantVel)
                    : deadZonePenalty
            );
        }
    }
}
