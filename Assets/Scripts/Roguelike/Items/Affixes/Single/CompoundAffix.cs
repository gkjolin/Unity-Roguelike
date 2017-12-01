using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;

namespace AKSaigyouji.Roguelike
{
    [CreateAssetMenu(fileName = "Compound Affix", menuName = "AKSaigyouji/Affixes/Compound")]
    public sealed class CompoundAffix : AffixDefinition
    {
        public IEnumerable<AffixDefinition> Affixes { get { return affixes; } }

        [SerializeField] AffixDefinition[] affixes;

        public override void OnEquip(IEquipContext context, float quality)
        {
            foreach (var affix in affixes)
            {
                affix.OnEquip(context, quality);
            }
        }
    }
}