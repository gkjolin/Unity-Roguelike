using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

namespace AKSaigyouji.Roguelike
{
    [CreateAssetMenu(fileName = "Health Potion", menuName = "AKSaigyouji/Items/Health Potion")]
    public sealed class PotionTemplate : ItemTemplate
    {
        public override InventorySlot Slot { get { return InventorySlot.Consumable; } }
        public int HealthRestored { get { return healthRestored; } }

        [SerializeField] int healthRestored;

        public override Item Build(string name)
        {
            return BuildPotion(name);
        }

        public Potion BuildPotion(string name)
        {
            return new Potion(this, name);
        }

        void OnValidate()
        {
            healthRestored = Mathf.Max(0, healthRestored);
        }
    }
}