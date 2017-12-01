﻿using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace AKSaigyouji.Roguelike
{
    /// <summary>
    /// Represents the destroyable body of an enemy.
    /// </summary>
    public class EnemyBody : MonoBehaviour, IAttackable, IHealable
    {
        public string Name { get { return name; } }

        int Health { get { return stats.CurrentHealth; } }
        int Defense { get { return stats.Defense; } }

        bool IsDead { get { return Health <= 0; } }

        EnemyStats stats;

        // We re-use this list for all enemies when they die.
        static List<IOnDeath> onDeathActions = new List<IOnDeath>();

        void Start()
        {
            Assert.IsNotNull(stats);
        }

        public void Initialize(EnemyStats stats)
        {
            this.stats = stats;
        }

        public virtual void Attack(int damageRoll, string attackText)
        {
            if (IsDead)
            {
                return; // don't beat a dead horse
            }
            stats.InflictDamage(damageRoll);
            string attackTextFormat = IsDead ? "You deal {0} damage, fatally wounding it." : "{0} damage dealt.";
            string attackResultText = string.Format(attackTextFormat, damageRoll);
            Logger.LogFormat("{0} {1}", attackText, attackResultText);
            if (IsDead)
            {
                // It's important we do this after we've logged the attack, otherwise any logging conducted by
                // the Die method would come before the attack.
                Die();
            }
        }

        public void Heal(int amount)
        {
            stats.InflictDamage(-amount);
        }

        void Die()
        {
            Assert.IsTrue(onDeathActions.Count == 0);
            GetComponents(onDeathActions);
            foreach (IOnDeath onDeathAction in onDeathActions)
            {
                onDeathAction.Invoke();
            }
            onDeathActions.Clear();
            gameObject.SetActive(false);
        }
    }
}