using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;

namespace AKSaigyouji.Roguelike
{
    // Sometimes coming up with the right verb is hard. 'Builder' and 'Factory' imply the design pattern, 'Summer' and
    // 'Adder' suggest something simpler, 'Collector' and 'Gatherer' imply mere grouping. Perhaps this difficulty suggests
    // the need for more than one class? Perhaps a separate class to group up all the sources of attributes, and another
    // to actually do the computations. Then again, creating more classes just to appease linguistic anxieties does
    // seem rather silly, doesn't it. Then again, no one will read this so I might as well be writing gibberish. 
    // Quixotic quakers quietly quack quickly. 

    /// <summary>
    /// Responsible for collecting attribu tes from all sources and calculating a final set of attributes to be used
    /// by the player.
    /// </summary>
    public sealed class AttributeAggregator : MonoBehaviour
    {
        public IndexedAttributes Attributes { get { return finalAttributes; } }

        [SerializeField] IndexedAttributes baseAttributes;
        [SerializeField] IndexedAttributes perLevelAttributes;

        [Header("For Visualization Purposes")]
        [SerializeField] IndexedAttributes finalAttributes;

        Dictionary<Attribute, StatBooster> statBoosters;

        PlayerEquipContext equipContext;

        void Awake()
        {
            finalAttributes = new IndexedAttributes();
            equipContext = new PlayerEquipContext();

            // We're using a normal dictionary, so it won't autopopulate with all the values of Attribute, so we copy
            // the keys from baseAttributes, and take the opportunity to initialize the stat boosters.
            statBoosters = new Dictionary<Attribute, StatBooster>();
            foreach (Attribute key in baseAttributes.Keys)
            {
                statBoosters.Add(key, new StatBooster());
            }
        }

        public void InventoryChangedEventHandler(IEnumerable<Item> newItems)
        {
            FlushOldData();
            ReapplyItemEffects(newItems);
            AggregateEnhancements();
            ApplyEnhancements();
        }

        void FlushOldData()
        {
            foreach (Attribute key in baseAttributes.Keys)
            {
                finalAttributes[key] = 0;
                statBoosters[key].Reset();
            }
            equipContext.Clear();
        }

        void ReapplyItemEffects(IEnumerable<Item> newItems)
        {
            foreach (Item item in newItems)
            {
                item.ApplyEffects(equipContext);
            }
        }

        void AggregateEnhancements()
        {
            foreach (AttributeEnhancement enhancement in equipContext.Enhancements)
            {
                statBoosters[enhancement.Attribute].AddBoost(enhancement.Priority, enhancement.Magnitude);
            }
        }

        void ApplyEnhancements()
        {
            foreach (Attribute attribute in baseAttributes.Keys)
            {
                finalAttributes[attribute] = ComputeBoostedValue(attribute);
            }
        }

        int ComputeBoostedValue(Attribute attribute)
        {
            int baseValue = baseAttributes[attribute] + perLevelAttributes[attribute];
            StatBooster booster = statBoosters[attribute];
            return booster.Boost(baseValue);
        }
    }
}