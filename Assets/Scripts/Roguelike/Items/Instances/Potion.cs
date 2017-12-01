using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

namespace AKSaigyouji.Roguelike
{
    [Serializable]
    public sealed class Potion : Item<PotionTemplate>, IConsumable
    {
        public int HealthRestored { get { return template.HealthRestored; } }

        public override string DisplayString { get { return displayString; } }

        readonly string displayString;

        public Potion(PotionTemplate template, string name) : base(template, name, Enumerable.Empty<Affix>())
        {
            displayString = string.Format("{0} health", HealthRestored);
        }

        public void Use(GameObject consumer)
        {
            IHealable target = consumer.GetComponent<IHealable>();
            if (target != null)
            {
                target.Heal(HealthRestored);
                Logger.LogFormat("Used health potion to restore {0} health.", HealthRestored);
            }
            else
            {
                Logger.Log("Target cannot be healed by potion.");
            }
        }

        public override Item Equip(IInventory inventory)
        {
            // this may be changed in the future - at the moment, potions should be handled separately 
            // by sniffing out the IConsumable interface, and consuming immediately. This may be changed to allow for
            // potions to be stored for later use, or even equipped as an item possible with affixes.
            throw new InvalidOperationException("Cannot equip a potion. Check CanEquip before attempting to equip.");
        }
    }
}