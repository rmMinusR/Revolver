using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(TimedDestroy))]
public class DamageNumberDriver : MonoBehaviour
{
    [SerializeField] private Vector3 riseRate;

    [Space]
    [SerializeField] private float totalStoredDamage;
    [SerializeField] private Color averagedColor;

    [Header("Damage color presets")]
    [SerializeField] private Color neutralColor = Color.white;
    [SerializeField] private Color fireColor = Color.red;
    [SerializeField] private Color acidColor = Color.green;
    [SerializeField] private Color shockColor = Color.blue;

    public void Accumulate(Damage damage)
    {
        Color damageColor = damage.element switch
        {
            Damage.Element.Neutral => neutralColor,
            Damage.Element.Fire => fireColor,
            Damage.Element.Acid => acidColor,
            Damage.Element.Shock => shockColor,
            _ => throw new System.NotImplementedException("Unknown Damage.Element: "+damage.element),
        };

        totalStoredDamage += damage.damageAmount;
        averagedColor = Color.Lerp(averagedColor, damageColor, damage.damageAmount/totalStoredDamage);
    }

    void Update()
    {
        transform.position += riseRate * Time.deltaTime;
        transform.forward = Camera.main.transform.forward;
    }
}
