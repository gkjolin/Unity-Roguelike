using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;

namespace AKSaigyouji.Roguelike
{
    [CreateAssetMenu(fileName = "Compound Affix", menuName = "AKSaigyouji/Affixes/Affix (Compound Attribute)", order = 3)]
    public sealed class CompoundAttributeAffix : AffixDefinition
    {
        public IEnumerable<AffixDefinition> Affixes { get { return affixes; } }
        public MagnitudeRangeAsset MagnitudeRange { get { return range; } }

        [SerializeField] AttributeAffix[] affixes;
        [SerializeField] MagnitudeRangeAsset range;

        public void Start()
        {
            Assert.IsNotNull(affixes);
            Assert.IsNotNull(range);
            Assert.IsTrue(affixes.Length > 0);
            Assert.IsTrue(affixes.Any(affix => affix != null));
        }

        public override string GetAffixDescription(QualityRoll quality)
        {
            return string.Format(affixFormatDescription, range.Value.Interpolate(quality));
        }

        public override void OnEquip(IEquipContext context, QualityRoll quality)
        {
            foreach (var affix in affixes.Where(aff => aff != null))
            {
                affix.OnEquip(context, quality, range.Value);
            }
        }
    }
}