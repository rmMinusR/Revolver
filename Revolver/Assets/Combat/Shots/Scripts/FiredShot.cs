using Combat;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.VFX;

public class FiredShot : MonoBehaviour, ICombatEffect
{
    [SerializeField] [Min(0)] private float lingerTime = 2;
    [SerializeField] Gradient colorOverTime;
    private float lingerTimeRemaining = 0;

    private RaycastHit hit;
    private ICombatTarget target;

    [SerializeField] [Range(0, 180)] private float magnetismAngle = 5f;
    [SerializeField] private List<Damage> effects;

    private LineRenderer trail;

    private void Start()
    {
        bool good = Physics.Raycast(transform.position, transform.forward, out hit);
        if (good) target = hit.collider.GetComponentInParent<ICombatTarget>();
        good &= target != null;

        //If we didn't hit anything, try to lock onto nearest target
        if (!good)
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

            //Redo raycast to make sure we can hit it
            else if (!Physics.Raycast(transform.position, ((Component)target).transform.position-transform.position, out hit)) target = null;
        }
        
        lingerTimeRemaining = lingerTime;

        //Setup trail
        trail = GetComponent<LineRenderer>();
        if (trail) trail.SetPosition(0, transform.position);
        if (trail) trail.SetPosition(1, hit.collider ? hit.point : transform.position+transform.forward*100);

        //Apply damage
        if (target != null && __source.GetSentimentTowards(target) != Sentiment.Ally) Apply(target);
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