using System;

namespace Combat
{
    /// <summary>
    /// An entity that deals damage or inflicts statuses: Player, enemy, poison dart trap
    /// </summary>
    public interface ICombatAffector
    {
        public Sentiment GetSentimentTowards(ICombatTarget other);
    }

    [Flags]
    public enum Group
    {
        PlayerCharacter   = 1 << 0,
        AttackerCharacter = 1 << 8
    }

    public enum Sentiment
    {
        Passive, //No interaction
        Ally,    //Can target with buffs
        Enemy    //Can target with damage
    }

}