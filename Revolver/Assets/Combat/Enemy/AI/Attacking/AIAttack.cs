using Combat;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AIAttack : MonoBehaviour
{
    [SerializeField] [Min(0)] protected internal float individualCooldown;
    protected internal float nextActivatableTime = 0;
    [SerializeField] [Min(0)] protected internal float globalCooldown;
    public bool IsValid => Time.time > nextActivatableTime;

    protected internal ICombatTarget target { get; private set; }
    protected internal AIAttackController context { get; private set; }

    public void BeginPerforming(AIAttackController context, ICombatTarget target)
    {
        this.target = target;
        this.context = context;

        _BeginPerforming();
    }

    protected abstract void _BeginPerforming();

    protected void OnFinishedPerforming()
    {
        nextActivatableTime = Time.time + individualCooldown;
        context.SetCooldown(globalCooldown);

        target = null;
        context = null;
    }
}
