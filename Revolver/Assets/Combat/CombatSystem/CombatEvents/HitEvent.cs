﻿using System;
using System.Collections.Generic;

namespace Combat
{
    //TODO fix coupling: requires on sequential dispatch. Remedied somewhat by API.

    [Serializable]
    public class HitEvent : CombatEvent
    {
        public readonly ICombatTarget target;
        public readonly ICombatEffect damagingEffect;

        public readonly IReadOnlyList<Damage> originalEffects;
        public List<Damage> effects;

        internal HitEvent(ICombatAffector source, ICombatTarget target, ICombatEffect damagingEffect, IEnumerable<Damage> effects) : base(source)
        {
            this.target = target;
            this.damagingEffect = damagingEffect;

            this.effects = new List<Damage>(effects);
            this.originalEffects = new List<Damage>(this.effects);
        }
    }
}
