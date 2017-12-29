using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Assertions;

namespace AKSaigyouji.Roguelike
{
    public sealed class AttackResult 
    {
        /// <summary>
        /// Target was successfully struck, and target was alive when attacked.
        /// </summary>
        public bool IsSuccess => !IsMiss && !IsEvaded && OriginalHealth > 0;

        /// <summary>
        /// Target was alive before attack, is dead after attack.
        /// </summary>
        public bool IsTargetKilled => OriginalHealth > 0 && FinalHealth <= 0;

        /// <summary>
        /// Total damage dealt.
        /// </summary>
        public int TotalDamage => PhysicalDamage.Final + MagicDamage.Final
            + FireDamage.Final + ColdDamage.Final + LightningDamage.Final + PoisonDamage.Final;

        /// <summary>
        /// Total health restored. 
        /// </summary>
        public int TotalHealing => PhysicalDamage.Absorbed + MagicDamage.Absorbed
            + FireDamage.Absorbed + ColdDamage.Absorbed + LightningDamage.Absorbed + PoisonDamage.Final;

        /// <summary>
        /// Attacker failed to strike target.
        /// </summary>
        public bool IsMiss { get; set; }

        /// <summary>
        /// Target avoided attack.
        /// </summary>
        public bool IsEvaded { get; set; }

        /// <summary>
        /// Attack was a critical hit.
        /// </summary>
        public bool IsCritical { get; set; }

        /// <summary>
        /// The target's health after the attack.
        /// </summary>
        public int FinalHealth => OriginalHealth - TotalDamage + TotalHealing;

        /// <summary>
        /// The target's health before the attack.
        /// </summary>
        public int OriginalHealth { get; set; }

        public DamageInfo PhysicalDamage { get; set; }

        public DamageInfo MagicDamage { get; set; }
        public DamageInfo FireDamage { get; set; }
        public DamageInfo ColdDamage { get; set; }
        public DamageInfo LightningDamage { get; set; }
        public DamageInfo PoisonDamage { get; set; }

        public string NameOfAttacker { get; set; }
        public string NameOfDefender { get; set; }

        public void Clear()
        {
            IsMiss = IsEvaded = IsCritical = false;
            OriginalHealth = 0;
            PhysicalDamage = MagicDamage = FireDamage = ColdDamage = LightningDamage = PoisonDamage = new DamageInfo();
            NameOfAttacker = NameOfDefender = null;
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendFormat("{0} attacks {1}\n", NameOfAttacker, NameOfDefender);
            if (IsSuccess)
            {
                if (IsCritical)
                {
                    sb.AppendLine("Critical hit!");
                }
                sb.AppendFormat("{0} damage dealt ({1} absorbed)\n", TotalDamage, TotalHealing);
                if (IsTargetKilled)
                {
                    sb.AppendFormat("{0} dies\n", NameOfDefender);
                }
            }
            else if (IsMiss)
            {
                sb.AppendFormat("{0} misses\n", NameOfAttacker);
            }
            else if (IsEvaded)
            {
                sb.AppendFormat("{1} evades the attack\n", NameOfDefender);
            }
            else
            {
                throw new InvalidOperationException("Unrecognized attack state.");
            }
            return sb.ToString();
        }
    } 
}