using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AKSaigyouji.Roguelike
{
    [Serializable]
    public class Shield : Item<ShieldTemplate>
    {
        public int Armor { get { return armor; } }

        public override string ItemDescription
        {
            get { return template.BuildDescription(Armor); }
        }

        [SerializeField] int armor;

        public Shield(ShieldTemplate template, string name, IEnumerable<Affix> affixes, ArmorEnhancement armorEnhancement) 
            : base(template, name, affixes)
        {
            armor = armorEnhancement.EnhanceArmor(template.Armor);
        }

        public override Item Equip(IInventory inventory)
        {
            Item old = inventory.Shield;
            inventory.Shield = this;
            return old;
        }
    } 
}