/* This class is designed to be the point through which other classes interact with all of the player's stats,
 both transient (current health, experience, etc.) and durable (max health, resistances, armor, etc). It hides
 the complexity in the fact that the stats are distributed across many systems (skills, items, base attributes, 
 etc.)*/

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AKSaigyouji.Roguelike
{
    /// <summary>
    /// Interface to all of the player's stats.
    /// </summary>
    public sealed class PlayerStats : MonoBehaviour
    {
        public int Level { get { return level; } }
        public int MaxHealth { get { return Attributes[Attribute.Health]; } }
        public int Armor { get { return Attributes[Attribute.Armor]; } }
        public float Speed { get { return speed; } }

        public int CurrentHealth { get { return currentHealth; } }
        public int Experience { get { return experience; } }

        IndexedAttributes Attributes { get { return attributeAggregator.Attributes; } }

        [SerializeField] AttributeAggregator attributeAggregator;

        [SerializeField] int level;
        [SerializeField] float speed;

        [SerializeField, ReadOnly] int currentHealth;
        [SerializeField, ReadOnly] int experience;

        // Levels go by 1000, 3000, 6000, 10000, etc (3rd edition D&D).
        bool HaveEnoughExperienceToLevel { get { return experience >= ExperienceToNextLevel; } }
        int ExperienceToNextLevel { get { return 500 * level * (level + 1); } }

        void Start()
        {
            currentHealth = MaxHealth;
        }

        public void AddExperience(int quantity)
        {
            if (quantity < 0)
                throw new ArgumentOutOfRangeException("quantity", "Cannot add negative experience.");

            experience += quantity;
            Logger.LogFormat("Gained {0} experience.", quantity);
            while (HaveEnoughExperienceToLevel) 
            {
                Logger.LogFormat("Level up! You are now level {0}.", level + 1);
                LevelUp();
                RestoreHealth();
            }
        }

        /// <summary>
        /// Adds the given quantity to the current health of the player. Use negative numbers to inflict damage,
        /// positive numbers to heal.
        /// </summary>
        public void ChangeHealth(int quantity)
        {
            currentHealth += quantity;
            currentHealth = Mathf.Clamp(currentHealth, 0, MaxHealth);
        }

        // Quick and dirty temporary solution.
        public string GetFormattedStatus()
        {
            return string.Format("Level: {0}\nHealth: {1}/{2}\nArmor: {3}\nSpeed: {4}\nExperience: {5}\nNext Level: {6}\n",
                level, currentHealth, MaxHealth, Armor, speed, experience, ExperienceToNextLevel)
                + string.Join("\n", Attributes.Select(pair => string.Format("{0}: {1}", pair.Key, pair.Value)).ToArray());
        }

        void OnValidate()
        {
            level = Mathf.Max(1, level);
            speed = Mathf.Max(0.001f, speed);
        }

        void LevelUp()
        {
            level++;
        }

        void RestoreHealth()
        {
            currentHealth = MaxHealth;
        }
    } 
}