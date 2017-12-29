using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

namespace AKSaigyouji.Roguelike
{
    public sealed class ItemFactory : MonoBehaviour
    {
        [SerializeField] AffixDatabase affixDatabase;

        [Header("Item Gen Variables")]
        [SerializeField] int maxSuffixesOnRare = 3;
        [SerializeField] int maxPrefixesOnRare = 3;

        [Tooltip("Probability of a normal item being upgraded to a magic item.")]
        [SerializeField, Range(0f, 1f)] float magicProbability = 0.5f;

        [Tooltip("Probability of a magic item being upgraded to a rare item.")]
        [SerializeField, Range(0f, 1f)] float rareProbability = 0.5f;

        [Tooltip("Probability of a rare item being upgraded to a unique item.")]
        [SerializeField, Range(0f, 1f)] float uniqueProbability = 0.5f;

        float RandomProbability { get { return UnityEngine.Random.Range(0f, 1f); } }

        public Item Build(ItemTemplate template)
        {
            if (template.Slot != InventorySlot.NotEquippable)
            {
                bool magicThresholdMet = RandomProbability < magicProbability;
                bool rareThresholdMet = RandomProbability < rareProbability;
                bool uniqueThresholdMet = RandomProbability < uniqueProbability;
                if (magicThresholdMet && rareThresholdMet && uniqueThresholdMet)
                {
                    return BuildUnique(template);
                }
                else if (magicThresholdMet && rareThresholdMet)
                {
                    return BuildRare(template);
                }
                else if (magicThresholdMet)
                {
                    return BuildMagic(template);
                }
            }
            return BuildMundane(template);
        }

        Item BuildMundane(ItemTemplate template)
        {
            template.StartBuilding();
            return template.FinishBuilding(template.Name);
        }

        Item BuildMagic(ItemTemplate template)
        {
            template.StartBuilding();
            var affixes = GetAffixesForRareItem(template.Slot, 1, 1);
            foreach (var affix in GetAffixesForRareItem(template.Slot, 1, 1))
            {
                template.AddAffix(affix, QualityRoll.GetRandom());
            }
            var prefix = affixes.First(aff => aff.IsPrefix);
            var suffix = affixes.First(aff => aff.IsSuffix);
            return template.FinishBuilding($"{prefix.Name} {template.Name} of {suffix.Name}");
        }

        Item BuildRare(ItemTemplate template)
        {
            template.StartBuilding();
            int numPrefixes = UnityEngine.Random.Range(1, maxSuffixesOnRare);
            int numSuffixes = UnityEngine.Random.Range(1, maxPrefixesOnRare);
            foreach (var affix in GetAffixesForRareItem(template.Slot, numPrefixes, numSuffixes))
            {
                template.AddAffix(affix, QualityRoll.GetRandom());
            }
            // Will most likely replace with a random name generator, similar in design to how the rare enemy
            // name generator works.
            return template.FinishBuilding($"Rare {template.Name}");
        }

        Item BuildUnique(ItemTemplate template)
        {
            throw new NotImplementedException();
        }

        List<AffixDefinition> GetAffixesForRareItem(InventorySlot slot, int numPrefixes, int numSuffixes)
        {
            AffixCollection collection;
            if (slot == InventorySlot.Weapon)
            {
                collection = affixDatabase.WeaponAffixes;
            }
            else if (slot == InventorySlot.BodyArmor || slot == InventorySlot.Shield)
            {
                collection = affixDatabase.ArmorAffixes;
            }
            else
            {
                throw new ArgumentException("Invalid item type for building magical item.", nameof(slot));
            }
            return affixDatabase.PickRandomAffixes(collection, numPrefixes, numSuffixes);
        }
    } 
}