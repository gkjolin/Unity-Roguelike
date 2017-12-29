/* This class was designed to handle a problem illustrated by the following example: Vitality and Health are defined
 by the same enum, and are thus handled at the same time in the same enum dictionary. But health depends on vitality:
 each point of vitality gives several points of health. Furthermore, such calculations are sensitive to order with
 respect to other modifications to health. e.g. if we have an affix that gives +10% health, we want vitality to be 
 applied first. This class handles this issue by handling the responsibility of converting stat boosters to stats,
 handling relationships between primary and secondary attributes.
 
  An alternative approach would be to split primary/secondary attributes into separate enums. Although this simplifies
 the design, it makes it prohibitively difficult to add/remove attribute dependencies later - moving an attribute from
 one enum to another will break a lot of serialized data. Hence we suffer a bit of added complexity in favour of 
 flexibility. We could have a step further and provided the ability to add dependencies completely in the editor,
 but such additions (or removals) should be infrequent enough that having to modify this class is acceptable, especially
 since the relationship may be sufficiently complex to require writing new code anyway.
 
  Adding a dependency is done by adding the attribute to the secondary attributes array, adding another ApplyXxxxxBoost
 method which provides the logic behind the dependency, and then executing the method in the ApplySecondaryBoosts methods.*/

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;

namespace AKSaigyouji.Roguelike
{
    /// <summary>
    /// Responsible for taking the base attributes, the collected stat boosters from various systems, and compiling
    /// them into final attributes. Internally handles dependencies between attributes. 
    /// </summary>
    public sealed class AttributeBoostApplyer
    {
        public IEnumerable<Attribute> SecondaryAttributes { get { return secondaryAttributes; } }

        readonly HashSet<Attribute> secondaryAttributes;

        readonly Attribute[] resistances = new[]
        {
            Attribute.FireResistance,
            Attribute.ColdResistance,
            Attribute.LightningResistance,
            Attribute.PoisonResistance
        };

        readonly Attribute[] primaryAttributes = new[]
        {
            Attribute.Strength,
            Attribute.Dexterity,
            Attribute.Magic,
            Attribute.Vitality
        };

        public AttributeBoostApplyer()
        {
            secondaryAttributes = new HashSet<Attribute>(primaryAttributes.Concat(resistances))
            {
                Attribute.Health,
            };
        }

        /// <summary>
        /// Takes the original, unboosted attributes, all the accumulated boosts aside from the primary-secondary
        /// interactions, and compiles them in-place. Will apply all the boosts and primary-secondary
        /// effects to produce final values for all the attributes.
        /// </summary>
        public void ApplyEnhancementsAndBuild(IndexedAttributes attributes, Dictionary<Attribute, StatBooster> boosters)
        {
            var primaryAttributes = boosters.Keys.Where(att => !secondaryAttributes.Contains(att));
            BuildAttributes(primaryAttributes, attributes, boosters);
            ApplyAndBuild(attributes, boosters);
        }

        void BuildAttributes(IEnumerable<Attribute> keys, IndexedAttributes attributes, 
            Dictionary<Attribute, StatBooster> boosters)
        {
            foreach (Attribute key in keys)
            {
                BuildAttribute(key, attributes, boosters);
            }
        }

        void ApplyAndBuild(IndexedAttributes attributes, Dictionary<Attribute, StatBooster> boosters)
        {
            foreach (Attribute attribute in primaryAttributes) // must occur before the health boost
            {
                ApplyAdditiveBoost(boosters[attribute], attributes[Attribute.AllAttributes]);
                BuildAttribute(attribute, attributes, boosters);
            }

            foreach (Attribute attribute in resistances)
            {
                ApplyAdditiveBoost(boosters[attribute], attributes[Attribute.AllResistance]);
                BuildAttribute(attribute, attributes, boosters);
            }

            ApplyHealthBoost(boosters[Attribute.Health], attributes[Attribute.Vitality], attributes[Attribute.HealthPerVitality]);
            BuildAttribute(Attribute.Health, attributes, boosters);
        }

        void ApplyHealthBoost(StatBooster booster, int vitality, int healthPerVitality)
        {
            ApplyAdditiveBoost(booster, healthPerVitality * vitality);
        }

        void ApplyAdditiveBoost(StatBooster booster, int value)
        {
            booster.AddBoost(EnhancementOperation.Additive, value, StatBooster.Priority.First);
        }

        void BuildAttribute(Attribute attribute, IndexedAttributes attributes, Dictionary<Attribute, StatBooster> boosters)
        {
            attributes[attribute] = boosters[attribute].Boost(attributes[attribute]);
        }
    } 
}