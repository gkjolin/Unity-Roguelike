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

        public override Item Build(string name)
        {
            return BuildWeapon(name);
        }

        public Weapon BuildWeapon(string name)
        {
            return new Weapon(this, name);
        }
    } 
}