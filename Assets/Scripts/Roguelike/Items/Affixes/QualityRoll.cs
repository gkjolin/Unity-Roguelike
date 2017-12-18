/* This type is mostly just a float with restricted values. Using a custom type allows it to manage its own invariants
 * (namely, the restriction on range of values). It may receive some functionality in the future. */

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;

namespace AKSaigyouji.Roguelike
{
    /// <summary>
    /// Represents a quality roll between 0 (worst) and 1 (perfect), e.g. for an affix on an item with a potential
    /// range of values: if an affix provides 5 - 25 of an attribute, a roll of 0 may correspond to 5, a roll of 25
    /// may correspond to 25, and intermediate values can be interpolated accordingly.
    /// </summary>
    [Serializable]
    public struct QualityRoll 
    {
        // we could have chosen integers as the backing value, e.g. from 0 to 100, but that would restrict granularity:
        // a stat with a range of 1 to 1000, for example, would not be able to take the value of 5. 
        public float Value { get { return quality; } }
        [SerializeField] float quality;

        public QualityRoll(float value)
        {
            if (value < -Mathf.Epsilon || value > 1 + Mathf.Epsilon) // some tolerance for floating point error
                throw new ArgumentException("Quality must be between 0 and 1 inclusive.");

            quality = Mathf.Clamp(value, 0, 1);
        }

        public static QualityRoll GetRandom()
        {
            return new QualityRoll(UnityEngine.Random.Range(0f, 1f));
        }
    } 
}