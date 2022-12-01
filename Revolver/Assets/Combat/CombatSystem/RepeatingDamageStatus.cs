using Combat;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class RepeatingDamageStatus : MonoBehaviour, ICombatEffect
{
    private ICombatTarget target;
    private float timeToNextTick = 0;
    private float timeRemaining;
    private float ticksPerSecond;

    private List<Damage> effectsPerTick;

    private ICombatAffector __source;
    public void SetSource(ICombatAffector source) => __source = source;
    public ICombatAffector GetSource() => __source;
    public void Apply(ICombatTarget target) => CombatAPI.Hit(GetSource(), target, this, effectsPerTick);

    private void Update()
    {
        timeToNextTick -= Time.deltaTime;
        if (timeToNextTick <= 0)
        {
            CombatAPI.Hit(GetSource(), target, this, effectsPerTick);
            timeToNextTick = 1 / ticksPerSecond;
        }

        timeRemaining -= Time.deltaTime;
        if (timeRemaining <= 0) Destroy(gameObject);
    }

    public static RepeatingDamageStatus Apply(ICombatAffector source, ICombatTarget target, float time, float ticksPerSecond, List<Damage> effectsPerTick, GameObject vfx)
    {
        GameObject go = new GameObject(nameof(RepeatingDamageStatus));
        if (source is Component c)
        {
            go.transform.parent = c.transform;
            if (vfx) Instantiate(vfx, go.transform);
        }

        RepeatingDamageStatus s = go.AddComponent<RepeatingDamageStatus>();
        s.SetSource(source);
        s.target = target;
        s.timeRemaining = time;
        s.ticksPerSecond = ticksPerSecond;
        s.effectsPerTick = effectsPerTick;

        return s;
    }
}
