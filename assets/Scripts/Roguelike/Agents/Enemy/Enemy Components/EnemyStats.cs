using UnityEngine;

namespace AKSaigyouji.Roguelike
{
    public class EnemyStats : MonoBehaviour
    {
        public int CurrentHealth { get { return currentHealth; } }
        public int MaxHealth { get { return maxHealth; } }
        public int Defense { get { return defense; } }
        public int Accuracy { get { return accuracy; } }
        public int MinDamage { get { return minDamage; } }
        public int MaxDamage { get { return maxDamage; } }

        [SerializeField] int maxHealth = 5;
        [SerializeField] int defense = 5;
        [SerializeField] int accuracy = 20;
        [SerializeField] int minDamage = 1;
        [SerializeField] int maxDamage = 2;

        [SerializeField, ReadOnly] int currentHealth;

        void Start()
        {
            currentHealth = maxHealth;
        }

        public void Promote(IEnemyEnhancement enhancement)
        {
            maxHealth = enhancement.EnhanceMaxHealth(maxHealth);
            defense = enhancement.EnhanceDefense(defense);
            accuracy = enhancement.EnhanceAccuracy(accuracy);
            minDamage = enhancement.EnhanceDamage(minDamage);
            maxDamage = enhancement.EnhanceDamage(maxDamage);
        }

        /// <summary>
        /// Use negative values to heal.
        /// </summary>
        public void InflictDamage(int quantity)
        {
            currentHealth -= quantity;
            currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        }

        void OnValidate()
        {
            maxHealth = Mathf.Max(1, maxHealth);
            defense = Mathf.Clamp(defense, 0, 100);
            accuracy = Mathf.Clamp(accuracy, 0, 100);
            minDamage = Mathf.Max(0, minDamage);
            maxDamage = Mathf.Max(minDamage, maxDamage);
        }
    } 
}