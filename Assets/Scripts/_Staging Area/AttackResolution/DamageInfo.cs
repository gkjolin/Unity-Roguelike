/* This class packages together the details of a single type of damage during the resolution of an attack. The level of 
 * granularity allows for a very detailed breakdown of an attack. This can be used for both diagnostic purposes as well
 * as in-game information, so a player understands how the various elements (resistance, absorption, reductions, etc.) 
 * are affecting the total results.*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AKSaigyouji.Roguelike
{
    /// <summary>
    /// Expresses the components of damage for a given type.
    /// </summary>
    public struct DamageInfo 
    {
        public int Raw { get; }

        /// <summary>
        /// The amount of damage prevented by percentage-based mitigation.
        /// </summary>
        public int Mitigated { get; }

        /// <summary>
        /// The amount of damage that was absorbed, healing the target. 
        /// </summary>
        public int Absorbed { get; }

        /// <summary>
        /// Flat reduction applied after mitigation.
        /// </summary>
        public int Reduced { get; }

        public int Final
        {
            get
            {
                return Mathf.Max(Raw - Mitigated - Reduced - Absorbed, 0);
            }
        }

        /// <param name="raw">Base damage before all mitigating factors.</param>
        /// <param name="mitigated">Percentage reduction from armor, resistance, etc.</param>
        /// <param name="absorbed">How much this attack heals the target.</param>
        /// <param name="reduced">Final flat reduction of the damage dealt.</param>
        public DamageInfo(int raw, int mitigated, int absorbed, int reduced)
        {
            Raw = raw;
            Mitigated = mitigated;
            Absorbed = absorbed;
            Reduced = Mathf.Min(reduced, raw - mitigated);
        }
    } 
}
