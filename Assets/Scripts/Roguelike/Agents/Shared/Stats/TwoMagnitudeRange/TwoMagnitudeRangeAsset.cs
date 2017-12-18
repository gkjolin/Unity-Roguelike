using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;

namespace AKSaigyouji.Roguelike
{
    [CreateAssetMenu(fileName = "Two Magnitude Range", menuName = "AKSaigyouji/Variables/Two Magnitude Range")]
    public sealed class TwoMagnitudeRangeAsset : ScriptableObject
    {
        public TwoMagnitudeRange Value { get { return value; } }
        [SerializeField] TwoMagnitudeRange value;
    } 
}