/* This wraps an affix definition with a quality value. A higher quality value represents a better roll, though
 it's up to the particular affix definition to decide how (if at all) the quality should affect the values associated
 with that affix. e.g. if an affix improves a stat by 20%-50%, it may use simple linear interpolation. If it adds
 (3-27) to (54-93) fire damage, then there's a wider variety of approaches. An obvious one is to interpolate both 
 the min and max figures separately. A more interesting one is to use the quality to determine the average damage, then 
 somehow use the quality as a seed to pick from all the ranges that give approximately that average. Finally, if the affix 
 provides +1 to all skills, then the quality is likely to be ignored, though perhaps an exceptional quality roll 
 (e.g. 0.99 or better) should provide +2.  */

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;

namespace AKSaigyouji.Roguelike
{
    /// <summary>
    /// Represents an instance of an affix. 
    /// </summary>
    [Serializable]
    public sealed class Affix
    {
        public string Name { get { return affix.Name; } }
        public AffixLocation Location { get { return affix.Location; } }
        public float Quality { get { return quality; } }

        [SerializeField] AffixDefinition affix;
        [SerializeField] float quality;

        public Affix(AffixDefinition definition, float quality)
        {
            affix = definition;
            this.quality = quality;
        }

        public void Equip(IEquipContext context)
        {
            affix.OnEquip(context, quality);
        }
    }
}