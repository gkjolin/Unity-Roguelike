using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

namespace AKSaigyouji.Roguelike
{
    [CreateAssetMenu(fileName = "Armor", menuName = "AKSaigyouji/Items/Armor")]
    public sealed class ArmorTemplate : ItemTemplate
    {
        public int Armor { get { return armor; } }

        public override InventorySlot Slot { get { return InventorySlot.BodyArmor; } }

        [SerializeField] int armor;

        public override Item Build(string name)
        {
            return BuildArmor(name);
        }

        public BodyArmor BuildArmor(string name)
        {
            return new BodyArmor(this, name);
        }
    } 
}