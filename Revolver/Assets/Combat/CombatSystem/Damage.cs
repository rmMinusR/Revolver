using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct Damage
{
    [field: SerializeField, Min(0)] public float damageAmount { get; set; }
    [field: SerializeField] public Element element { get; set; }

    public enum Element
    {
        Neutral = 0,
        Fire,
        Acid,
        Shock
    }
}
