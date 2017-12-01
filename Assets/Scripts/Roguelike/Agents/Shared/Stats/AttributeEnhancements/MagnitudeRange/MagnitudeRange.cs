using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;

namespace AKSaigyouji.Roguelike
{
    [Serializable]
    public struct MagnitudeRange
    {
        public int Min { get { return min; } }
        public int Max { get { return max; } }

        [SerializeField] int min;
        [SerializeField] int max;

        public MagnitudeRange(int min, int max)
        {
            if (min > max)
                throw new ArgumentException("Min must not exceed max.");

            this.min = min;
            this.max = max;
        }

        /// <summary>
        /// Gets a value between min and max (both inclusive), based on the quality, a float between 0 (min) and 1 (max).
        /// </summary>
        public int GetValue(float percentQuality)
        {
            if ((percentQuality < - Mathf.Epsilon) || (percentQuality > 1 + Mathf.Epsilon))
                throw new ArgumentOutOfRangeException("percentQuality", "Quality must be between 0 and 1 inclusive.");

            return (int)Mathf.Lerp(min, max, percentQuality);
        }
    } 
}