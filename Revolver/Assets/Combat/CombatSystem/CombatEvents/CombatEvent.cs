using Events;
using System;

namespace Combat
{
    internal interface ICombatEvent
    {
    }

    [Serializable]
    public abstract class CombatEvent : PubSubEvent, ICombatEvent
    {
        public ICombatAffector source;

        protected internal CombatEvent(ICombatAffector source)
        {
            this.source = source;
        }
    }
}
