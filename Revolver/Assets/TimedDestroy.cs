using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class TimedDestroy : MonoBehaviour
{
    [Min(0)] public float timeToLive = 2;

    private void Update()
    {
        timeToLive -= Time.deltaTime;
        if (timeToLive <= 0) Destroy(gameObject);
    }
}
