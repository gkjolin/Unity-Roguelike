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

        int Armor { get { return stats.Armor + inventory.BodyArmor.Armor + inventory.Shield.Armor; } }
        int Health { get { return stats.CurrentHealth; } }
        
        const string missText = "Miss!";
        const string strikeTextFormat = "{0} damage dealt. {1} health remaining.";
        const string deathTextFormat = "{0} damage dealt. You are fatally wounded!";

        void Start()
        {
            Assert.IsNotNull(stats);
            Assert.IsNotNull(inventory);
        }

        public void Heal(int amount)
        {
            stats.ChangeHealth(amount);
        }

        public void Attack(int damageRoll, string attackText)
        {
            if (Health < 1) // already dead, do nothing
            {
                return;
            }
            string attackResultText;
            stats.ChangeHealth(-damageRoll);
            if (Health < 1)
            {
                attackResultText = string.Format(deathTextFormat, damageRoll);
                Lose();
            }
            else
            {
                attackResultText = string.Format(strikeTextFormat, damageRoll, Health);
            }
            Logger.LogFormat("{0} {1}", attackText, attackResultText);
        }

        void Lose()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}