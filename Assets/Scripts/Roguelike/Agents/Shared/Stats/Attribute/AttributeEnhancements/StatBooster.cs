/* The attribute system is fairly complex, with buffs and penalties coming in from various sources at various times.
 The order in which the buffs and penalties will affect the end values greatly, so it's useful to be able to aggregate
 the various enhancements for a stat in a single place, and then apply them at once, in the correct order. This class
 allows for such an aggregation, in a relatively light-weight fashion. */

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
        // An option I considered was to make StatBooster either an EnumDictionary or have it wrap one. 
        // This would simplify the class, but with a considerably larger footprint.
        [SerializeField] int firstAdditive;
        [SerializeField] int firstMultiplicative;
        [SerializeField] int secondAdditive;
        [SerializeField] int secondMultiplicative;
        [SerializeField] int finalAdditive;
        [SerializeField] int finalMultiplicative;
        [SerializeField] int overrideValue;

        [SerializeField] bool isOverrideSet; // 0 is a default override value, so need to check separately 

        /// <summary>
        /// Add an enhancement to this statbooster. Priority must correspond to one of the valid stat boosts. 
        /// </summary>
        public void AddBoost(EnhancementPriority priority, int magnitude)
        {
            switch (priority)
            {
                case EnhancementPriority.FirstAdditive:
                    firstAdditive += magnitude;
                    break;
                case EnhancementPriority.FirstMultiplicative:
                    firstMultiplicative += magnitude;
                    break;
                case EnhancementPriority.SecondAdditive:
                    secondAdditive += magnitude;
                    break;
                case EnhancementPriority.SecondMultiplicative:
                    secondMultiplicative += magnitude;
                    break;
                case EnhancementPriority.FinalAdditive:
                    finalAdditive += magnitude;
                    break;
                case EnhancementPriority.FinalMultiplicative:
                    finalMultiplicative += magnitude;
                    break;
                case EnhancementPriority.Override:
                    isOverrideSet = true;
                    overrideValue = magnitude;
                    break;
                default:
                    throw new System.ComponentModel.InvalidEnumArgumentException();
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