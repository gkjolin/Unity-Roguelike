using System;
using System.Collections;
using System.Collections.Generic;

namespace AKSaigyouji.Roguelike
{
    [Serializable]
    public class Weapon : Item<WeaponTemplate>
    {
        public int MinDamage { get { return template.MinDamage; } }
        public int MaxDamage { get { return template.MaxDamage; } }
        public int CritMultiplier { get { return template.CritMultiplier; } }

        public override string DisplayString { get { return displayString; } }

        readonly string displayString;

        const string displayStringFormat = "{0}-{1} damage"
            + "\n" + "x{2} crit multiplier";

        public Weapon(WeaponTemplate template, string name, IEnumerable<Affix> affixes) 
            : base(template, name, affixes)
        {
            displayString = string.Format(displayStringFormat, MinDamage, MaxDamage, CritMultiplier);
        }

        public override Item Equip(IInventory inventory)
        {
            Weapon item = inventory.Weapon;
            inventory.Weapon = this;
            return item;
        }
    } 
}