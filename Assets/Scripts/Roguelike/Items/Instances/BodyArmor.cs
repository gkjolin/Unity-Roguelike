using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AKSaigyouji.Roguelike
{
    [Serializable]
    public class BodyArmor : Item<ArmorTemplate>
    {
        public int Armor { get { return armor; } }

        [SerializeField] int armor;

        public override string ItemDescription
        {
            get { return Template.BuildDescription(Armor); }
        }

        public BodyArmor(ArmorTemplate template, string name, IEnumerable<Affix> affixes, ArmorEnhancement armorEnhancement)
            : base(template, name, affixes)
        {
            armor = armorEnhancement.EnhanceArmor(template.Armor);
        }

        public override Item Equip(IInventory inventory)
        {
            BodyArmor old = inventory.BodyArmor;
            inventory.BodyArmor = this;
            return old;
        }
    } 
}