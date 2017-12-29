/* The attribute system is fairly complex, with buffs and penalties coming in from various sources at various times.
 The order in which the buffs and penalties will affect the end values greatly, so it's useful to be able to aggregate
 the various enhancements for a stat in a single place, and then apply them at once, in the correct order. This class
 encapsulates such aggregations, in a relatively light-weight fashion. */

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;

namespace AKSaigyouji.Roguelike
{
    [Serializable]
    /// <summary>
    /// Represents an aggegated boost for a single stat. Can be serialized, but is designed to be usable without
    /// doing so. 
    /// </summary>
    public sealed class StatBooster
    {
        public enum Priority { First = 1, Second = 2, Final = 200}

        // could have used a collection, but this is a lot more explicit (makes the class easier to understand)
        // note that the order of these fields is also the order of the respective operations
        // the idea behind the tiers is that the first tier is for attributes on items, the second is for 
        // skills, spells/curses and similar effects, while the final tier is for environmental effects,
        // such as the global reduction to resistances from higher difficulties in Diablo 2, and possibly also
        // for shrines (though shrines might be a good candidate for spell-like effects). 
        [SerializeField] int firstAdditive;
        [SerializeField] int firstMultiplicative;
        [SerializeField] int secondAdditive;
        [SerializeField] int secondMultiplicative;
        [SerializeField] int finalAdditive;
        [SerializeField] int finalMultiplicative;
        [SerializeField] int overrideValue;

        [SerializeField] bool isOverrideSet;

        /// <summary>
        /// Add an enhancement to this statbooster. Priority must correspond to one of the valid stat boosts. 
        /// </summary>
        public void AddBoost(EnhancementOperation operation, int magnitude, Priority priority)
        {
            bool first = priority == Priority.First;
            bool second = priority == Priority.Second;
            bool final = priority == Priority.Final;

            bool add = operation == EnhancementOperation.Additive;
            bool mult = operation == EnhancementOperation.Multiplicative;
            bool overridden = operation == EnhancementOperation.Override;

            Assert.IsTrue(first || second || final);
            Assert.IsTrue(add || mult || overridden);

            if (first && add)
            {
                firstAdditive += magnitude;
            }
            else if (first && mult)
            {
                firstMultiplicative += magnitude;
            }
            else if (second && add)
            {
                secondAdditive += magnitude;
            }
            else if (second && mult)
            {
                secondMultiplicative += magnitude;
            }
            else if (final && add)
            {
                finalAdditive += magnitude;
            }
            else if (final && mult)
            {
                finalMultiplicative += magnitude;
            }
            else if (overridden)
            {
                overrideValue = isOverrideSet ? Mathf.Min(overrideValue, magnitude) : magnitude;
                isOverrideSet = true;
            }
            else
            {
                // In the editor, the assertion above will fail. At run-time, the unidentified boost is skipped.
            }
        }

        public int Boost(int originalValue)
        {
            if (isOverrideSet)
            {
                return overrideValue;
            }
            else
            {
                // We use floating point intermediate values to avoid intermediate truncations,
                // and doubles specifically for more precision. Only for the final value do we truncate
                // and return an integer.
                double firstPass = (originalValue + firstAdditive) * (1 + (firstMultiplicative * 0.01));
                double secondPass = (firstPass + secondAdditive) * (1 + (secondMultiplicative * 0.01));
                double finalPass = (secondPass + finalAdditive) * (1 + (finalMultiplicative * 0.01));
                return (int)finalPass;
            }
        }

        public void Reset()
        {
            firstAdditive = 0;
            firstMultiplicative = 0;
            secondAdditive = 0;
            secondMultiplicative = 0;
            finalAdditive = 0;
            finalMultiplicative = 0;
            overrideValue = 0;
            isOverrideSet = false;
        }
    } 
}