using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

namespace AKSaigyouji.Roguelike
{
    [CreateAssetMenu(fileName = "Health Potion", menuName = "AKSaigyouji/Items/Health Potion")]
    public sealed class PotionTemplate : ItemTemplate
    {
        public override InventorySlot Slot { get { return InventorySlot.NotEquippable; } }
        public int HealthRestored { get { return healthRestored; } }

        [SerializeField] int healthRestored;

        public override Item Build(ItemBuildContext context)
        {
            return new Potion(this, name);
        }

        void OnValidate()
        {
            healthRestored = Mathf.Max(0, healthRestored);
        }
    }
}