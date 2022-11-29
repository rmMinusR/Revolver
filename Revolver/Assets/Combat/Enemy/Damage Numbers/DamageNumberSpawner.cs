using Combat;
using Events;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class DamageNumberSpawner : ScopedListener
{
    [SerializeField] private CombatantEntity owner;
    [SerializeField] private DamageNumberDriver prefab;
    [SerializeField] [Min(0)] private float maxMergeTime;

    private DamageNumberDriver display;
    private float lastSpawnedTime = float.NegativeInfinity;

    [EventHandler(Priority.Final)]
    private void SpawnDamageNumbers(HitEvent ev)
    {
        if (ev.target is CombatantEntity ent && ent == owner)
        {

            float sinceLast = Time.time-lastSpawnedTime;

            if (display == null || sinceLast > maxMergeTime)
            {
                display = Instantiate(prefab, owner.transform.position, Quaternion.identity);
            }

            foreach (Damage e in ev.effects) display.Accumulate(e);
            
        }
    }
}
