using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.SceneManagement;

namespace AKSaigyouji.Roguelike
{
    public sealed class PlayerBody : MonoBehaviour, IAttackable, IHealable
    {
        public string Name { get { return name; } }

        [SerializeField] PlayerStats stats;
        [SerializeField] Inventory inventory;

        int ItemArmor { get { return inventory.BodyArmor.Armor + inventory.Shield.Armor; } }
        
        void Start()
        {
            Assert.IsNotNull(stats);
            Assert.IsNotNull(inventory);
        }

        public void Heal(int amount)
        {
            Assert.IsTrue(amount >= 0);
            stats.ChangeHealth(amount);
        }

        public void Damage(int amount)
        {
            Assert.IsTrue(amount >= 0);
            stats.ChangeHealth(-amount);
        }

        public void Attack(AttackResult attack)
        {
            if (attack.IsSuccess)
            {
                Damage(attack.TotalDamage);
                Heal(attack.TotalHealing);
            }
        }

        public Defense GetDefense()
        {
            var defense = new Defense()
            {
                NameOfDefender = name,

                Armor = stats.GetAttribute(Attribute.Armor) + ItemArmor,
                CurrentHealth = stats.CurrentHealth,
                MaxHealth = stats.GetAttribute(Attribute.Health),

                FireResistance = stats.GetAttribute(Attribute.FireResistance),
                ColdResistance = stats.GetAttribute(Attribute.ColdResistance),
                LightningResistance = stats.GetAttribute(Attribute.LightningResistance),
                PoisonResistance = stats.GetAttribute(Attribute.PoisonResistance),

                FireAbsorption = 0,
                ColdAbsorption = 0,
                LightningAbsorption = 0,
                PoisonAbsoprtion = 0,

                PhysicalDamageSubtraction = 0,
                NonPhysicalDamageSubstraction = 0,

                PhysicalEvasion = 0,
                MagicalEvasion = 0,
            };
            return defense;
        }

        void Lose()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}