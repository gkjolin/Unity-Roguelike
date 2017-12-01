using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;

namespace AKSaigyouji.Roguelike
{
    [CreateAssetMenu(fileName = "Attribute Affix", menuName = "AKSaigyouji/Affixes/Attribute")]
    public sealed class AttributeAffix : AffixDefinition
    {
        public Attribute Attribute { get { return attribute; } }
        public EnhancementPriority Priority { get { return priority; } }
        public MagnitudeRangeAsset Range { get { return rangeVariable; } }

        [SerializeField] Attribute attribute;
        [SerializeField] EnhancementPriority priority;
        [SerializeField] MagnitudeRangeAsset rangeVariable;

        public override void OnEquip(IEquipContext context, float quality)
        {
            context.ApplyAttributeBuff(BuildEnhancement(quality));
        }

        public AttributeEnhancement BuildEnhancement(float quality)
        {
            if (rangeVariable == null)
                throw new InvalidOperationException(string.Format("Range not specified in {0} attribute.", Name));

            return new AttributeEnhancement(attribute, priority, rangeVariable.Value.GetValue(quality));
        }
    } 
}