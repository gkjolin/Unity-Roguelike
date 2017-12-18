using System;
using System.Collections;
using System.Collections.Generic;

namespace AKSaigyouji.Roguelike
{
    [Serializable]
    public class Weapon : Item<WeaponTemplate>
    {
        public int MinDamage { get { return minDamage; } }
        public int MaxDamage { get { return maxDamage; } }
        public int CritMultiplier { get { return critMultiplier; } }

        public override string ItemDescription
        {
            get { return template.BuildDescription(MinDamage, MaxDamage, CritMultiplier); }
        }

        readonly int minDamage;
        readonly int maxDamage;
        readonly int critMultiplier;

        public Weapon(WeaponTemplate template, string name, IEnumerable<Affix> affixes, WeaponEnhancement enhancer) 
            : base(template, name, affixes)
        {
            minDamage = enhancer.EnhanceMinDamage(template.MinDamage);
            maxDamage = enhancer.EnhanceMaxDamage(template.MaxDamage);
            critMultiplier = enhancer.EnhanceCrit(template.CritMultiplier);
        }

        public override Item Equip(IInventory inventory)
        {
            Weapon item = inventory.Weapon;
            inventory.Weapon = this;
            return item;
        }
    } 
}