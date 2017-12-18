using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;

namespace AKSaigyouji.Roguelike
{
    /// <summary>
    /// Reference to a serialized magnitude range.
    /// </summary>
    [CreateAssetMenu(fileName = "Magnitude Range", menuName = "AKSaigyouji/Variables/Magnitude Range")]
    public sealed class MagnitudeRangeAsset : ScriptableObject
    {
        public MagnitudeRange Value { get { return range; } }

        [SerializeField] MagnitudeRange range;
    } 
}