using System;

namespace AKSaigyouji.Roguelike
{
    /// <summary>
    /// Representation of a buff, curse, or other modification to an entity's stats.
    /// </summary>
    public struct StatModification
    {
        /// <summary>
        /// The stat affected by this modification.
        /// </summary>
        public readonly Stat stat;

        readonly float multiplier;
        readonly float offset;

        /// <summary>
        /// Create a new stat modifier that shifts a value by an offset and then multiplies it by a coefficient.
        /// </summary>
        /// <param name="stat"></param>
        /// <param name="priority"></param>
        /// <param name="offset"></param>
        /// <param name="multiplier"></param>
        public StatModification(Stat stat, float offset = 0, float coefficient = 1)
        {
            this.stat = stat;
            this.offset = offset;
            multiplier = coefficient;
        }

        // Stats actually tend to be integers, but by using floats we handle intermediate calculations better. 
        // e.g. if we start with a value of 3 and have ten buffs that increase the value by 10%, we should end up
        // with 6, but would end up with 6 if we truncate/round on each calculation

        public float Apply(float initialValue)
        {
            return multiplier * initialValue + offset;
        }
    } 
}