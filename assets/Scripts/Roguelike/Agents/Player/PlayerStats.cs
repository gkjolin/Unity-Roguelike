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

        public int CurrentHealth { get { return currentHealth; } }
        public int Experience { get { return experience; } }
        public int ExperienceToNextLevel { get { return 500 * level * (level + 1); } }

        IndexedAttributes Attributes { get { return attributeAggregator.Attributes; } }

        [SerializeField] AttributeAggregator attributeAggregator;

        [SerializeField] int level;

        [SerializeField, ReadOnly] int currentHealth;
        [SerializeField, ReadOnly] int experience;

        // Levels go by 1000, 3000, 6000, 10000, etc (3rd edition D&D, quadratic growth). Quadratic growth naturally
        // makes farming lower-level enemies inefficient without imposing special penalties.
        bool HaveEnoughExperienceToLevel { get { return experience >= ExperienceToNextLevel; } }

        const int HEALTH_PER_VIT = 3;

        void Start()
        {
            currentHealth = GetAttribute(Attribute.Health);
        }
        
        public int GetAttribute(Attribute attribute)
        {
            return Attributes[attribute];
        }

        public void AddExperience(int quantity)
        {
            if (quantity < 0)
                throw new ArgumentOutOfRangeException("quantity", "Cannot add negative experience.");

            experience += quantity;
            Logger.LogFormat("Gained {0} experience.", quantity);
            while (HaveEnoughExperienceToLevel) // though unlikely, it's possible to gain multiple levels at once
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
            currentHealth = Mathf.Clamp(currentHealth, 0, GetAttribute(Attribute.Health));
        }

        void LevelUp()
        {
            level++;
        }

        void RestoreHealth()
        {
            currentHealth = GetAttribute(Attribute.Health);
        }
    } 
}