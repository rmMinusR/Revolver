using Combat;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public sealed class AIAttackController : MonoBehaviour
{
    public AICharacterHost host { get; private set; }

    [SerializeField] private ISteeringProvider betweenAttacksStrategy;

    private List<AIAttack> attacks;

    internal void BindTo(AICharacterHost host)
    {
        this.host = host;
        attacks = GetComponentsInChildren<AIAttack>().ToList();

        host.steering.controller = betweenAttacksStrategy;
    }

    public AIAttack currentlyPerforming { get; private set; }

    private void Update()
    {
        if (currentlyPerforming == null && host.attackTarget != null && Time.time > nextAttackTime)
        {
            //Attempt to resolve a valid Attack
            List<AIAttack> valid = attacks.Where(i => i.IsValid).ToList();

            //If at least one can be performed, choose and perform a random one
            if (valid.Count != 0)
            {
                currentlyPerforming = valid[UnityEngine.Random.Range(0, valid.Count-1)];
                currentlyPerforming.BeginPerforming(this, host.attackTarget);
            }
        }
    }

    private float nextAttackTime;
    internal void SetCooldown(float globalCooldown)
    {
        nextAttackTime = Time.time + globalCooldown;
        currentlyPerforming = null;
        host.steering.controller = betweenAttacksStrategy;
    }
}
