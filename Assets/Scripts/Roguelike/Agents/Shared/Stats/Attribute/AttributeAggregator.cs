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
    // Quixotic quakers questing quietly quack quickly. 

    /// <summary>
    /// Responsible for collecting attributes from all sources and calculating a final set of attributes to be used
    /// by the player.
    /// </summary>
    public sealed class AttributeAggregator : MonoBehaviour
    {
        public IndexedAttributes Attributes { get { return finalAttributes; } }

        // We can't modify a dictionary while enumerating over its keys, so we'll just reference the keys in 
        // the unchanging baseAttributes whenever we need to enumerate over any of these attribute dictionaries.
        IEnumerable<Attribute> AllKeys { get { return baseAttributes.Keys; } }

        [SerializeField] IndexedAttributes baseAttributes;
        [SerializeField] IndexedAttributes perLevelAttributes;

        [Tooltip("Calculated automatically at run-time in response to attribute changing events.")]
        [SerializeField] IndexedAttributes finalAttributes;

        Dictionary<Attribute, StatBooster> statBoosters;

        PlayerEquipContext equipContext;
        AttributeBoostApplyer boostApplyer;
       
        void Awake()
        {
            finalAttributes = new IndexedAttributes();
            equipContext = new PlayerEquipContext();
            boostApplyer = new AttributeBoostApplyer();

            // We're using a normal dictionary, so it won't autopopulate with all the values of Attribute, so we copy
            // the keys from baseAttributes, and take the opportunity to initialize the stat boosters.
            statBoosters = new Dictionary<Attribute, StatBooster>();
            foreach (Attribute key in AllKeys)
            {
                statBoosters.Add(key, new StatBooster());
            }

            InventoryChangedEventHandler(Enumerable.Empty<Item>());
        }

        // This should be wired up in the Unity editor.
        public void InventoryChangedEventHandler(IEnumerable<Item> newItems)
        {
            FlushOldData();
            ReapplyItemEffects(newItems);
            AggregateEnhancements();
            ApplyEnhancements();
        }

        void FlushOldData()
        {
            foreach (Attribute key in AllKeys)
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
            finalAttributes.AddAttributes(baseAttributes);
            finalAttributes.AddAttributes(perLevelAttributes);
            boostApplyer.ApplyEnhancementsAndBuild(finalAttributes, statBoosters);
        }
    }
}