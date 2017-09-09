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

        public Potion(PotionTemplate template, string name) : base(template, name)
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
        }
    }
}