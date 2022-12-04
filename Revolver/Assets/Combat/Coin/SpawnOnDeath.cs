using Combat;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CombatantEntity))]
public class SpawnOnDeath : MonoBehaviour
{
    private CombatantEntity ent;

    [SerializeField] private GameObject prefab;
    [SerializeField] [Min(1)] private int count = 3;
    [SerializeField] [Min(0)] private float scatterRange = 0.5f;

    private void Start()
    {
        ent = GetComponent<CombatantEntity>();
        ent.OnDeath += OnDeath;
    }

    private void OnDeath()
    {
        for (int i = 0; i < count; ++i)
        {
            Instantiate(prefab, transform.position + Random.insideUnitSphere*scatterRange, Random.rotationUniform);
        }
    }
}
