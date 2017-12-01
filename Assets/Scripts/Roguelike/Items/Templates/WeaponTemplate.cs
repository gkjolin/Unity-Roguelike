using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

namespace AKSaigyouji.Roguelike
{
    [CreateAssetMenu(fileName = "Weapon", menuName = "AKSaigyouji/Items/Weapon")]
    public sealed class WeaponTemplate : ItemTemplate
    {
        public int MinDamage { get { return minDamage; } }
        public int MaxDamage { get { return maxDamage; } }
        public int CritMultiplier { get { return critMultiplier; } }

        public override InventorySlot Slot { get { return InventorySlot.Weapon; } }

        [SerializeField] int minDamage;
        [SerializeField] int maxDamage;
        [SerializeField] int critMultiplier;

        public override Item Build(ItemBuildContext context)
        {
            if (context.NumAffixes > 0)
            {
                var affixDB = context.AffixDatabase;
                var affixes = affixDB.WeaponAffixes;
                var prefixes = affixes.Affixes.Where(aff => aff.Location == AffixLocation.Prefix).ToArray();
                var suffixes = affixes.Affixes.Where(aff => aff.Location == AffixLocation.Suffix).ToArray();
                List<Affix> chosenAffixes = new List<Affix>();
                Affix prefix = ChooseRandomAffix(prefixes);
                Affix suffix = ChooseRandomAffix(suffixes);
                chosenAffixes.Add(prefix);
                chosenAffixes.Add(suffix);
                return new Weapon(this, string.Format("{0} {1} of {2}", prefix.Name, Name, suffix.Name), chosenAffixes);
            }
            else
            {
                return new Weapon(this, Name, Enumerable.Empty<Affix>());
            }
        }

        Affix ChooseRandomAffix(AffixDefinition[] affixDefinitions)
        {
            return new Affix(ChooseRandomAffixDefinition(affixDefinitions), GetRandomQuality());
        }

        AffixDefinition ChooseRandomAffixDefinition(AffixDefinition[] affixes)
        {
            return affixes[UnityEngine.Random.Range(0, affixes.Length)];
        }

        float GetRandomQuality()
        {
            return UnityEngine.Random.Range(0f, 1f);
        }
    } 
}