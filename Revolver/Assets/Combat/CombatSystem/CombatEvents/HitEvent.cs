using System;

namespace Combat
{
    //TODO fix coupling: requires on sequential dispatch. Remedied somewhat by API.

    [Serializable]
    public class HitEvent : CombatEvent
    {
        public readonly ICombatTarget target;
        public readonly ICombatEffect damagingEffect;

        public readonly float originalDamage;
        public float damage;

        internal HitEvent(ICombatAffector source, ICombatTarget target, ICombatEffect damagingEffect, float damage) : base(source)
        {
            this.target = target;
            this.damagingEffect = damagingEffect;

            this.originalDamage = damage;
            this.damage = damage;
        }
    }
}
