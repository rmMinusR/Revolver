using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Bullet : MonoBehaviour
{
    [SerializeField] protected GameObject onFired;

    public virtual void Fire(Transform origin)
    {
        Instantiate(onFired, origin.position, origin.rotation);
    }
}
