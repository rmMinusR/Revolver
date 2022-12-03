using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class AIGunAttack : AIAttack
{
    [Space]
    [SerializeField] private ISteeringProvider aimSteering;
    [SerializeField] [Min(0)] private float aimTime = 2;

    [Space]
    [SerializeField] private ISteeringProvider shootSteering;
    [SerializeField] [Min(1)] private int timesToShoot = 3;
    [SerializeField] [Min(0)] private float timeBetweenShots = 0.75f;
    [SerializeField] private AIShot shotPrefab;
    [SerializeField] private Transform shotSpawnRoot;

    [Space]
    [SerializeField] private ISteeringProvider recoverySteering;
    [SerializeField] [Min(0)] private float recoveryTime = 2;

    protected override void _BeginPerforming() => StartCoroutine(Attack());

    private IEnumerator Attack()
    {
        context.host.steering.controller = aimSteering;
        yield return new WaitForSeconds(aimTime);

        for (int i = 0; i < timesToShoot; ++i)
        {
            if (i != 0) yield return new WaitForSeconds(timeBetweenShots);
            Shoot();
        }

        yield return new WaitForSeconds(recoveryTime);

        OnFinishedPerforming();
    }

    private void Shoot()
    {
        AIShot shot = Instantiate(shotPrefab, shotSpawnRoot.position, Quaternion.identity);
        shot.transform.LookAt(((Component)context.host.attackTarget).transform);
        shot.homingTarget = ((Component)context.host.attackTarget).transform;
        shot.SetSource(context.host.combat);
    }
}
