using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace AKSaigyouji.Roguelike
{
    public sealed class PlayerAttack : MonoBehaviour
    {
        [SerializeField] Inventory inventory;
        [SerializeField] PlayerStats stats;

        int MinDamage { get { return inventory.Weapon.MinDamage; } }
        int MaxDamage { get { return inventory.Weapon.MaxDamage; } }
        int CritMultiplier { get { return inventory.Weapon.CritMultiplier; } }
        int Accuracy { get { return stats.Accuracy; } }

        readonly string NORMAL_ATTACK_FORMAT = "You attack {0}.";
        readonly string CRITICAL_ATTACK_FORMAT = "You deliver a critical hit to {0}.";

        void Start()
        {
            Assert.IsNotNull(inventory);
        }

        public void Attack(IAttackable target)
        {
            string targetName = target.Name;
            int baseAttack = UnityEngine.Random.Range(0, 100);
            int totalAttack = baseAttack + Accuracy;
            int damage = UnityEngine.Random.Range(MinDamage, MaxDamage);
            string attackFormat;
            if (baseAttack >= 95) // 5% chance to crit. Hardcoded for now, maybe turn it into a stat later.
            {
                totalAttack *= 10; // Extremely high chance to land a hit when delivering a critical.
                damage *= CritMultiplier;
                attackFormat = CRITICAL_ATTACK_FORMAT;
            }
            else
            {
                attackFormat = NORMAL_ATTACK_FORMAT;
            }
            target.Attack(totalAttack, damage, string.Format(attackFormat, targetName));
        }
    } 
}