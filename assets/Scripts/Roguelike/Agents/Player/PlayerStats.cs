using System;
using UnityEngine;

namespace AKSaigyouji.Roguelike
{
    public sealed class PlayerStats : MonoBehaviour
    {
        public int Level { get { return level; } }
        public int MaxHealth { get { return maxHealth; } }
        public int Armor { get { return armor; } }
        public int Accuracy { get { return accuracy; } }
        public float Speed { get { return speed; } }

        public int CurrentHealth { get { return currentHealth; } }
        public int Experience { get { return experience; } }

        [SerializeField] int level;
        [SerializeField] int maxHealth;
        [SerializeField] int armor;
        [SerializeField] int accuracy;
        [SerializeField] float speed;

        [SerializeField] int healthPerLevel;
        [SerializeField] int armorPerLevel;
        [SerializeField] int accuracyPerLevel;

        [SerializeField, ReadOnly] int currentHealth;
        [SerializeField, ReadOnly] int experience;

        // Levels go by 1000, 3000, 6000, 10000, etc (3rd edition D&D).
        bool HaveEnoughExperienceToLevel { get { return experience >= ExperienceToNextLevel; } }
        int ExperienceToNextLevel { get { return 500 * level * (level + 1); } }

        void Start()
        {
            currentHealth = maxHealth;
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
            currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        }

        public string GetFormattedStatus()
        {
            return string.Format("Level: {0}\nHealth: {1}/{2}\nArmor: {3}\nSpeed: {4}\nExperience: {5}\nNext Level: {6}",
                level, currentHealth, maxHealth, armor, speed, experience, ExperienceToNextLevel);
        }

        void OnValidate()
        {
            level = Mathf.Max(1, level);
            maxHealth = Mathf.Max(1, maxHealth);
            armor = Mathf.Max(0, armor);
            accuracy = Mathf.Max(0, accuracy);
            speed = Mathf.Max(0.001f, speed);

            healthPerLevel = Mathf.Max(0, healthPerLevel);
            armorPerLevel = Mathf.Max(0, armorPerLevel);
            accuracyPerLevel = Mathf.Max(0, accuracyPerLevel);
        }

        void LevelUp()
        {
            level++;
            maxHealth += healthPerLevel;
            armor += armorPerLevel;
            accuracy += accuracyPerLevel;
        }

        void RestoreHealth()
        {
            currentHealth = maxHealth;
        }
    } 
}