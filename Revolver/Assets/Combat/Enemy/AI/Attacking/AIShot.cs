using Combat;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class AIShot : MonoBehaviour, ICombatEffect
{
    [SerializeField] [Min(0)] private float homingStrength = 0;
    public Transform homingTarget;

    [SerializeField] [Min(0)] private float speed = 8;

    [SerializeField] private List<Damage> effects;

    private ICombatAffector __source;
    public void SetSource(ICombatAffector source) => __source = source;
    public ICombatAffector GetSource() => __source;
    public void Apply(ICombatTarget target) => CombatAPI.Hit(GetSource(), target, this, effects);

    private void Update()
    {
        if (homingTarget) transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(homingTarget.position-transform.position), homingStrength*Time.deltaTime);
        transform.position += transform.forward * speed * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject != ((Component)__source).gameObject && other.TryGetComponent(out ICombatTarget t))
        {
            Apply(t);
            Destroy(gameObject);
        }
    }
}
