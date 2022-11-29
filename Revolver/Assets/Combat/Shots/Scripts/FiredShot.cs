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
        //Do raycast
        Physics.Raycast(transform.position, transform.forward, out hit);

        //If we didn't hit anything, try to lock onto nearest target
        if (!hit.collider || !hit.collider.TryGetComponent(out target))
        {
            target = CombatantEntity.Instances //Should this be FindObjectsOfType instead?
                            .Select(e => (e, Vector3.Angle(transform.position-e.transform.position, transform.forward)))
                            .Where(e => e.Item2 < magnetismAngle)
                            .MinBy(e => e.Item2).e;

            //Redo raycast
            if (!Physics.Raycast(transform.position, ((Component)target).transform.position-transform.position, out hit)) target = null;
        }
        
        lingerTimeRemaining = lingerTime;

        //Setup trail
        trail = GetComponent<LineRenderer>();
        if (trail) trail.SetPosition(0, transform.position);
        if (trail) trail.SetPosition(1, hit.point);

        //Apply damage
        Apply(target);
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