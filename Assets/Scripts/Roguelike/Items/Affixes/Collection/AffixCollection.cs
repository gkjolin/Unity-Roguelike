using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;

namespace AKSaigyouji.Roguelike
{
    /// <summary>
    /// Base class for assets that manage a collection of affixes.
    /// </summary>
    public abstract class AffixCollection : ScriptableObject
    {
        public abstract IEnumerable<AffixDefinition> Affixes { get; }
        public abstract int Count { get; }
        public abstract AffixDefinition this[int index] { get; }
    } 
}