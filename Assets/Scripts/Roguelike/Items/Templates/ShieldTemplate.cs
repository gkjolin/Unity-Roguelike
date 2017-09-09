using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

namespace AKSaigyouji.Roguelike
{
    [CreateAssetMenu(fileName = "Shield", menuName = "AKSaigyouji/Items/Shield")]
    public sealed class ShieldTemplate : ItemTemplate
    {
        public int Armor { get { return armor; } }

        public override InventorySlot Slot { get { return InventorySlot.Shield; } }

        [SerializeField] int armor;

        public override Item Build(string name)
        {
            return BuildShield(name);
        }

        public Shield BuildShield(string name)
        {
            return new Shield(this, name);
        }
    }
}