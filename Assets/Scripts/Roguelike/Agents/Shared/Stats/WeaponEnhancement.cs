using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;

namespace AKSaigyouji.Roguelike
{
    /// <summary>
    /// Used internally by weapon-based items to manage weapon-enhancing affixes during the item-building process.
    /// </summary>
    public sealed class WeaponEnhancement
    {
        readonly StatBooster minBooster = new StatBooster();
        readonly StatBooster maxBooster = new StatBooster();
        readonly StatBooster critBooster = new StatBooster();

        public static bool IsWeaponAttribute(Attribute attribute)
        {
            return attribute == Attribute.MinDamage
                || attribute == Attribute.MaxDamage
                || attribute == Attribute.CritMultiplier;
        }

        public void ApplyWeaponAttribute(Attribute attribute, EnhancementPriority priority, int value)
        {
            Assert.IsTrue(priority == EnhancementPriority.FirstAdditive 
                       || priority == EnhancementPriority.FirstMultiplicative
                       || priority == EnhancementPriority.Override,
                       "Item enhancements must be first-priority, since they always precede other effects.");
            Assert.IsTrue(IsWeaponAttribute(attribute));
            if (attribute == Attribute.MinDamage)
            {
                minBooster.AddBoost(priority, value);
            }
            else if (attribute == Attribute.MaxDamage)
            {
                maxBooster.AddBoost(priority, value);
            }
            else if (attribute == Attribute.CritMultiplier)
            {
                critBooster.AddBoost(priority, value);
            }
        }

        public int EnhanceMinDamage(int original)
        {
            return minBooster.Boost(original);
        }

        public int EnhanceMaxDamage(int original)
        {
            return maxBooster.Boost(original);
        }

        public int EnhanceCrit(int original)
        {
            return critBooster.Boost(original);
        }

        public void Clear()
        {
            minBooster.Reset();
            maxBooster.Reset();
            critBooster.Reset();
        }
    }
}