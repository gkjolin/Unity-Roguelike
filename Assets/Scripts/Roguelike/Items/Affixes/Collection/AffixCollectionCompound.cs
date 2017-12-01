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
    [CreateAssetMenu(fileName = "Affix Collection", menuName = "AKSaigyouji/Affixes/Compound Collection")]
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
        public override AffixDefinition this[int index]
        {
            get
            {
                foreach (var collection in affixCollections)
                {
                    if (index < collection.Count)
                    {
                        return collection[index];
                    }
                    else
                    {
                        index -= collection.Count;
                    }
                }
                throw new ArgumentOutOfRangeException("index");
            }
        }

        [SerializeField] AffixCollection[] affixCollections;
    }
}