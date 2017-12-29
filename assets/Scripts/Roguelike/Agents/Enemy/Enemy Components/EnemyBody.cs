using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace AKSaigyouji.Roguelike
{
    /// <summary>
    /// Represents the destroyable body of an enemy.
    /// </summary>
    public sealed class EnemyBody : MonoBehaviour, IAttackable, IHealable
    {
        public string Name { get { return name; } }

        bool IsDead { get { return stats.CurrentHealth <= 0; } }

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

        public Defense GetDefense()
        {
            var defense = new Defense()
            {
                NameOfDefender = name,

                CurrentHealth = stats.CurrentHealth,
                MaxHealth = stats.MaxHealth,
                Armor = 25
            };
            return defense;
        }

        public void Attack(AttackResult attack)
        {
            if (attack.IsSuccess)
            {
                stats.InflictDamage(attack.TotalDamage);
                Heal(attack.TotalHealing);
                if (IsDead)
                {
                    Die();
                }
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