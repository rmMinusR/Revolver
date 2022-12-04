using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class RandomizeVelocityOnSpawn : MonoBehaviour
{
    [SerializeField] private Vector3 baseVel;
    [SerializeField] private Vector3 randomVel;
    [SerializeField] [Min(0)] private float randomRotation;

    private void Start()
    {
        Rigidbody rb = GetComponent<Rigidbody>();

        rb.velocity = baseVel + Vector3.Scale(Random.insideUnitSphere, randomVel);
        rb.angularVelocity = new Vector3(Random.value, Random.value, Random.value) * randomRotation;
    }
}
