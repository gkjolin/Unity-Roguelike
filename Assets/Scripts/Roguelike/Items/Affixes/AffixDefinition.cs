using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;

namespace AKSaigyouji.Roguelike
{
    public abstract class AffixDefinition : ScriptableObject
    {
        /// <summary>
        /// The name of the affix which can appear in the final item name, e.g. Flaming, Strong, The Stars, etc.
        /// </summary>
        public virtual string Name { get { return affixName; } }

        /// <summary>
        /// Represents the order-priority of this affix description in an item's affix list.
        /// </summary>
        public abstract int Order { get; } // Temporary: I have a better idea in mind for handling this later.

        /// <summary>
        /// Affix occupies a prefix slot. All affixes are either prefix or suffix.
        /// </summary>
        public bool IsPrefix { get { return location == AffixLocation.Prefix; } }

        /// <summary>
        /// Affix occupies a suffix slot. All affixes are either prefix or suffix.
        /// </summary>
        public bool IsSuffix { get { return location != AffixLocation.Prefix; } }

        [SerializeField] protected string affixFormatDescription;
        [SerializeField] string affixName;
        [SerializeField] AffixLocation location;

        /// <summary>
        /// The description of the affix appearing in the item's player-facing list of affixes, e.g. "+3 to strength",
        /// "+19 to all resistances", "5% chance to cast level 3 Tornado". 
        /// </summary>
        public abstract string GetAffixDescription(QualityRoll quality);

        public abstract void OnEquip(IEquipContext context, QualityRoll quality);
    } 
}