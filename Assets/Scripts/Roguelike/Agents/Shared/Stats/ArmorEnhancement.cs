using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;

namespace AKSaigyouji.Roguelike
{
    /// <summary>
    /// Used internally by armor-based items to manage armor-enhancing affixes during the item-building process.
    /// </summary>
    public sealed class ArmorEnhancement 
    {
        readonly StatBooster armor = new StatBooster();

        public static bool IsArmorAttribute(Attribute attribute)
        {
            return attribute == Attribute.Armor;
        }

        public void ApplyArmorAttribute(Attribute attribute, EnhancementPriority priority, int value)
        {
            Assert.IsTrue(priority == EnhancementPriority.FirstAdditive
                       || priority == EnhancementPriority.FirstMultiplicative
                       || priority == EnhancementPriority.Override,
                       "Item enhancements must be first-priority, since they always precede other effects.");
            Assert.IsTrue(IsArmorAttribute(attribute));
            if (attribute == Attribute.Armor)
            {
                armor.AddBoost(priority, value);
            }
        }

        public int EnhanceArmor(int original)
        {
            return armor.Boost(original);
        }

        public void Clear()
        {
            armor.Reset();
        }
    } 
}