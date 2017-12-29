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

        protected override string ItemDescriptionFormat
        {
            get { return "Restores {0} Health"; }
        }

        [SerializeField] int healthRestored;

        public string BuildDescription(int healthRestored)
        {
            return string.Format(ItemDescriptionFormat, healthRestored);
        }

        protected override Item OnFinishBuilding(List<Affix> affixes, string name)
        {
            return new Potion(this, name);
        }
    }
}