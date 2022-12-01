using Combat;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AISteeringHost))]
[RequireComponent(typeof(CombatantEntity))]
public sealed class AICharacterHost : MonoBehaviour
{
    [SerializeField] private ISteeringProvider passiveStrategy;
    [SerializeField] private ISteeringProvider attackStrategy;

    [SerializeField] private Sensor sensor;
    private AISteeringHost steering;
    private CombatantEntity combat;

    private ICombatTarget attackTarget;

    private void Start()
    {
        steering = GetComponent<AISteeringHost>();
        combat = GetComponent<CombatantEntity>();

        sensor.OnBeganSensing += AggroOnPlayer;
        steering.controller = passiveStrategy;
    }

    private void AggroOnPlayer(Sensable s)
    {
        if (s.TryGetComponent(out ICombatTarget t) && combat.GetSentimentTowards(t) == Sentiment.Hostile)
        {
            attackTarget = t;
        }
    }

    private void Update()
    {
        steering.controller = attackTarget != null ? attackStrategy : passiveStrategy;
    }
}
