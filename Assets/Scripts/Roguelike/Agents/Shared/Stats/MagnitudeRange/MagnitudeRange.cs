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

        public int Interpolate(QualityRoll quality)
        {
            return (int)Mathf.Lerp(min, max, quality.Value);
        }
    } 
}