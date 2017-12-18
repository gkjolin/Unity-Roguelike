using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;

namespace AKSaigyouji.Roguelike
{
    public interface IAffixCollection<T> where T : AffixDefinition
    {
        IEnumerable<T> Affixes { get; }
        int Count { get; }
    } 
}