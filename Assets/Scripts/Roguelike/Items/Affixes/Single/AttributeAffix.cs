using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;

namespace AKSaigyouji.Roguelike
{
    [CreateAssetMenu(fileName = "Attribute Affix", menuName = "AKSaigyouji/Affixes/Affix (Attribute)", order = 3)]
    public sealed class AttributeAffix : AffixDefinition
    {
        public Attribute Attribute { get { return attribute; } }
        public EnhancementOperation Priority { get { return priority; } }
        public MagnitudeRangeAsset Range { get { return rangeVariable; } }
        public override int Order { get { return (int)attribute; } }

        [SerializeField] Attribute attribute;
        [SerializeField] EnhancementOperation priority;
        [SerializeField] MagnitudeRangeAsset rangeVariable;

        public override string GetAffixDescription(QualityRoll quality)
        {
            return string.Format(affixFormatDescription, rangeVariable.Value.Interpolate(quality));
        }

        public override void OnEquip(IEquipContext context, QualityRoll quality)
        {
            if (rangeVariable == null)
                throw new InvalidOperationException("Magnitude range not supplied to AttributeAffix.");

            OnEquip(context, quality, rangeVariable.Value);
        }

        /*
        This overload is provided to allow for a simple run-time change to the value. Originally this
        was implemented to allow a compound attribute to substitute a different value for the range without
        having to rebuild new attribute affixes for every sub-attribute. e.g. a compound affix that improves
        all primary stats would have references to an affix for each primary stat, but we want to provide lesser
        values for the range for balance reasons (otherwise, the compound attribute would be 4x as strong as the
        individual affixes).
        */

        /// <summary>
        /// Provide a different value for the magnitude range than the one attached to this affix.
        /// </summary>
        public void OnEquip(IEquipContext context, QualityRoll quality, MagnitudeRange rangeOverride)
        {
            context.ApplyAttributeBuff(BuildEnhancement(quality, rangeOverride));
        }

        AttributeEnhancement BuildEnhancement(QualityRoll quality, MagnitudeRange range)
        {
            return new AttributeEnhancement(attribute, priority, range.Interpolate(quality));
        }
    } 
}