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
        /// Affix occupies a prefix slot. All affixes are either prefix or suffix.
        /// </summary>
        public bool IsPrefix { get { return location == AffixLocation.Prefix; } }

        // It's possible for an enum to take values outside of the provided options (e.g. x = (AffixLocation)9), 
        // so defining IsSuffix this way avoids the possibility of both IsPrefix and IsSuffix returning false.

        /// <summary>
        /// Affix occupies a suffix slot. All affixes are either prefix or suffix.
        /// </summary>
        public bool IsSuffix { get { return location != AffixLocation.Suffix; } }

        [SerializeField] protected string affixFormatDescription;

        [SerializeField] string affixName;

        // Easy way to get a nice, readable drop-down in the editor.
        [SerializeField] AffixLocation location;

        /// <summary>
        /// The description of the affix appearing in the item's user-facing list of affixes, e.g. "+3 to strength",
        /// "+19 to all resistances", "5% chance to cast level 3 Tornado". 
        /// </summary>
        public abstract string GetAffixDescription(QualityRoll quality);

        public abstract void OnEquip(IEquipContext context, QualityRoll quality);
    } 
}