using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;

namespace AKSaigyouji.Roguelike
{
    /// <summary>
    /// Like a magnitude range, but for two-dimensional stats, mainly intended for values with both a min 
    /// and a max. e.g. given a weapon damage of 2 to 12, a two magnitude range can be used to represent a buff of
    /// +((1-4) to (5-8)) damage. A particular instance may be 2 to 6, for a total of 4 to 18 damage.  
    /// </summary>
    [Serializable]
    public struct TwoMagnitudeRange 
    {
        public MagnitudeRange First { get { return first; } }
        public MagnitudeRange Second { get { return second; } }

        [SerializeField] MagnitudeRange first;
        [SerializeField] MagnitudeRange second;

        public TwoMagnitudeRange(MagnitudeRange first, MagnitudeRange second)
        {
            this.first = first;
            this.second = second;
        }
    } 
}