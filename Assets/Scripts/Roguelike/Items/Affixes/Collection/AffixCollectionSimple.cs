﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;

namespace AKSaigyouji.Roguelike
{
    /// <summary>
    /// A simple list of affixes.
    /// </summary>
    [CreateAssetMenu(fileName = "Affix Collection", menuName = "AKSaigyouji/Affixes/Collection")]
    public sealed class AffixCollectionSimple : AffixCollection
    {
        public override int Count { get { return affixes.Length; } }
        public override IEnumerable<AffixDefinition> Affixes { get { return affixes; } }
        public override AffixDefinition this[int index] { get { return affixes[index]; } }

        [SerializeField] AffixDefinition[] affixes;
    }
}