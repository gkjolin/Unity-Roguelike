using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;

namespace AKSaigyouji.Roguelike
{
	public sealed class PlayerAttacker : MonoBehaviour
	{
        [SerializeField] Inventory inventory;
        [SerializeField] PlayerStats stats;

        Weapon Weapon { get { return inventory.Weapon; } }
        
        void Start()
        {
            Assert.IsNotNull(inventory);
            Assert.IsNotNull(stats);
        }

        public Attack BuildAttack()
        {
            var attack = new Attack()
            {
                NameOfAttacker = name,

                Accuracy = 100,
                CritChance = 5,
                CritMultiplier = Weapon.CritMultiplier + stats.GetAttribute(Attribute.CritMultiplier),
                PhysicalDamage = ComputePhysicalDamage(),

                ColdDamage = new MagnitudeRange(),
                FireDamage = new MagnitudeRange(),
                LightningDamage = new MagnitudeRange(),
                PoisonDamage = new MagnitudeRange(),
                MagicDamage = new MagnitudeRange()
            };
            return attack;
        }

        // this computation is a bit too involved to do inline 
        MagnitudeRange ComputePhysicalDamage()
        {
            int weaponDamage = stats.GetAttribute(Attribute.WeaponDamage);

            int minMultiplierBoost = Weapon.Template.MinDamage * weaponDamage / 100;
            int minDamage = Weapon.MinDamage + minMultiplierBoost + stats.GetAttribute(Attribute.MinDamage);

            int maxMultiplierBoost = Weapon.Template.MaxDamage * weaponDamage / 100;
            int maxDamage = Weapon.MaxDamage + maxMultiplierBoost + stats.GetAttribute(Attribute.MaxDamage);

            maxDamage = Mathf.Max(minDamage, maxDamage); // Attribute.MinDamage might boost min above max

            return new MagnitudeRange(minDamage, maxDamage);
        }
    }
}