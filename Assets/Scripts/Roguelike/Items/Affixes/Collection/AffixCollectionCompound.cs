using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;

namespace AKSaigyouji.Roguelike
{
    /// <summary>
    /// Affix collection that works by aggregating other collections recursively.
    /// </summary>
    [CreateAssetMenu(fileName = "Affix Collection", menuName = "AKSaigyouji/Affixes/Collection (Compound)", order = 5)]
    public sealed class AffixCollectionCompound : AffixCollection
    {
        public override int Count
        {
            get
            {
                return affixCollections.Sum(col => col.Count);
            }
        }

        public override IEnumerable<AffixDefinition> Affixes
        {
            get
            {
                return affixCollections.SelectMany(col => col.Affixes);
            }
        }

        [SerializeField] AffixCollection[] affixCollections;
    }
}