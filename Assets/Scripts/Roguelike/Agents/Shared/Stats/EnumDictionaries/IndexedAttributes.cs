using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;

namespace AKSaigyouji.Roguelike
{
    [Serializable]
    public sealed class IndexedAttributes : EnumDictionary<Attribute, int>
    {
        public void AddAttributes(IEnumerable<KeyValuePair<Attribute, int>> attributes)
        {
            foreach (var pair in attributes)
            {
                this[pair.Key] += pair.Value;
            }
        }
    } 
}