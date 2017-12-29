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
        readonly StatBooster damageBooster = new StatBooster();
        readonly StatBooster critBooster = new StatBooster();
        readonly StatBooster speedBooster = new StatBooster();

        public static bool IsWeaponAttribute(Attribute attribute)
        {
            return attribute == Attribute.MinDamage
                || attribute == Attribute.MaxDamage
                || attribute == Attribute.WeaponDamage
                || attribute == Attribute.AttackSpeed
                || attribute == Attribute.CritMultiplier;
        }

        public void ApplyWeaponAttribute(Attribute attribute, EnhancementOperation operation, int value)
        {
            Assert.IsTrue(IsWeaponAttribute(attribute));
            if (attribute == Attribute.MinDamage)
            {
                AddBoost(minBooster, operation, value);
            }
            else if (attribute == Attribute.MaxDamage)
            {
                AddBoost(maxBooster, operation, value);
            }
            else if (attribute == Attribute.WeaponDamage)
            {
                AddBoost(damageBooster, operation, value);
            }
            else if (attribute == Attribute.AttackSpeed)
            {
                AddBoost(speedBooster, operation, value);
            }
            else if (attribute == Attribute.CritMultiplier)
            {
                AddBoost(critBooster, operation, value);
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

        public int EnhanceAttackSpeed(int original)
        {
            return speedBooster.Boost(original);
        }

        public int EnhanceCrit(int original)
        {
            return critBooster.Boost(original);
        }

        public void Clear()
        {
            minBooster.Reset();
            maxBooster.Reset();
            speedBooster.Reset();
            critBooster.Reset();
        }

        void AddBoost(StatBooster booster, EnhancementOperation operation, int value)
        {
            // weapon enhancements are all first-priority
            booster.AddBoost(operation, value, StatBooster.Priority.First);
        }
    }
}