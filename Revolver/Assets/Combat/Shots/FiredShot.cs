using Combat;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.VFX;

[DefaultExecutionOrder(-10)]
public class FiredShot : MonoBehaviour, ICombatEffect
{
    [SerializeField] [Min(0)] private float lingerTime = 2;
    [SerializeField] Gradient colorOverTime;
    private float lingerTimeRemaining = 0;

    [NonSerialized] public RaycastHit hit;
    [NonSerialized] public ICombatTarget target;

    [SerializeField] [Range(0, 180)] private float magnetismAngle = 5f;
    [SerializeField] private List<Damage> effects;

    private LineRenderer trail;

    [InspectorReadOnly] public bool isEmpowered = false;

    private void Start()
    {
        bool directHit = Physics.Raycast(transform.position, transform.forward, out hit);
        if (directHit) target = hit.collider.GetComponentInParent<ICombatTarget>();
        directHit &= target != null;

        //If we didn't hit anything, try to lock onto nearest target
        if (!directHit)
        {
            float minAngle = float.PositiveInfinity;
            foreach (CombatantEntity e in FindObjectsOfType<CombatantEntity>()) //TODO how to work with breakables?
            {
                float ang = Vector3.Angle(transform.position - e.transform.position, transform.forward);
                if (ang < minAngle)
                {
                    target = e;
                    minAngle = ang;
                }
            }

            //Verify it's within magnetism angle range
            if (minAngle > magnetismAngle) target = null;
            else
            {
                //Redo raycast to make sure we can hit it
                if (!Physics.Raycast(transform.position, ((Component)target).transform.position - transform.position, out hit)) target = null;
            }
        }

        //Ensure good hit location, even if firing at the air
        if (hit.collider == null) hit.point = transform.position + transform.forward * 100;

        //Setup trail
        lingerTimeRemaining = lingerTime;
        trail = GetComponent<LineRenderer>();
        if (trail) trail.SetPosition(0, transform.position);
        if (trail) trail.SetPosition(1, hit.point);

        //Apply damage
        if (target != null && __source.GetSentimentTowards(target) != Sentiment.Friendly) Apply(target);
    }

    private ICombatAffector __source;
    public void SetSource(ICombatAffector source) => __source = source;
    public ICombatAffector GetSource() => __source;
    public void Apply(ICombatTarget target) => CombatAPI.Hit(GetSource(), target, this, effects);


    private void Update()
    {
        lingerTimeRemaining -= Time.deltaTime;
        if (lingerTimeRemaining < 0) Destroy(gameObject);

        if (trail) trail.startColor = trail.endColor = colorOverTime.Evaluate(1 - lingerTimeRemaining/lingerTime);
    }
}