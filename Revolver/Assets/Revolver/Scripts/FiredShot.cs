using System.Collections;
using UnityEngine;
using UnityEngine.VFX;

[RequireComponent(typeof(VisualEffect))]
public class FiredShot : MonoBehaviour
{
    [SerializeField] [Min(0)] private float lingerTime = 2;
    private float lingerTimeRemaining = 0;

    private void Start()
    {
        lingerTimeRemaining = lingerTime;
    }

    private void Update()
    {
        lingerTimeRemaining -= Time.deltaTime;
        if (lingerTimeRemaining < 0) Destroy(gameObject);
    }
}