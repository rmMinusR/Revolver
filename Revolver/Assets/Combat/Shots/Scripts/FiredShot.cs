using System.Collections;
using UnityEngine;
using UnityEngine.VFX;

public class FiredShot : MonoBehaviour
{
    [SerializeField] [Min(0)] private float lingerTime = 2;
    [SerializeField] Gradient colorOverTime;
    private float lingerTimeRemaining = 0;

    private RaycastHit hit;

    private LineRenderer trail;

    private void Start()
    {
        //Do raycast
        Physics.Raycast(transform.position, transform.forward, out hit);

        lingerTimeRemaining = lingerTime;

        //Setup trail
        trail = GetComponent<LineRenderer>();
        if (trail) trail.SetPosition(0, transform.position);
        if (trail) trail.SetPosition(1, hit.point);
    }

    private void Update()
    {
        lingerTimeRemaining -= Time.deltaTime;
        if (lingerTimeRemaining < 0) Destroy(gameObject);

        if (trail) trail.startColor = trail.endColor = colorOverTime.Evaluate(1 - lingerTimeRemaining/lingerTime);
    }
}