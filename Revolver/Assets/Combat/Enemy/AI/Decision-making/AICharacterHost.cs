using Combat;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AISteeringHost))]
[RequireComponent(typeof(CombatantEntity))]
public sealed class AICharacterHost : MonoBehaviour
{
    [SerializeField] private ISteeringProvider passiveStrategy;

    [SerializeField] private Sensor sensor;
    public AISteeringHost steering { get; private set; }
    [SerializeField] private AIAttackController attackController;
    public CombatantEntity combat { get; private set; }

    public ICombatTarget attackTarget { get; private set; }

    private void Start()
    {
        steering = GetComponent<AISteeringHost>();
        combat = GetComponent<CombatantEntity>();

        attackController.BindTo(this);

        sensor.OnBeganSensing += AggroOnTarget;
        steering.controller = passiveStrategy;
    }

    private void AggroOnTarget(Sensable s)
    {
        if (s.TryGetComponent(out ICombatTarget t) && combat.GetSentimentTowards(t) == Sentiment.Hostile)
        {
            attackTarget = t;
        }
    }
}
