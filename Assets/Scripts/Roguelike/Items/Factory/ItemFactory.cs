using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

namespace AKSaigyouji.Roguelike
{
    public sealed class ItemFactory : MonoBehaviour
    {
        [SerializeField] AffixDatabase affixDatabase;

        public Item Build(ItemTemplate template)
        {
            template.StartBuilding();
            if (template.Slot != InventorySlot.NotEquippable)
            {
                foreach (var affix in GetAffixesForRareItem(3, 3))
                {
                    template.AddAffix(affix, QualityRoll.GetRandom());
                }
            }
            return template.FinishBuilding(template.Name);
        }

        IEnumerable<AffixDefinition> GetAffixesForRareItem(int numPrefixes, int numSuffixes)
        {
            return affixDatabase.PickRandomAffixes(affixDatabase.GeneralAffixes, numPrefixes, numSuffixes);
        }
    } 
}