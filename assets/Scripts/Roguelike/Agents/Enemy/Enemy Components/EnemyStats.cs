using UnityEngine;

namespace AKSaigyouji.Roguelike
{
    public class EnemyStats : MonoBehaviour
    {
        public int CurrentHealth { get { return currentHealth; } }
        public int MaxHealth { get { return maxHealth; } }
        public int Defense { get { return defense; } }
        public int MinDamage { get { return minDamage; } }
        public int MaxDamage { get { return maxDamage; } }

        [SerializeField] int maxHealth = 5;
        [SerializeField] int defense = 5;
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
            minDamage = enhancement.EnhanceDamage(minDamage);
            maxDamage = enhancement.EnhanceDamage(maxDamage);
        }

        /// <summary>
        /// Use negative values to heal.
        /// </summary>
        public void InflictDamage(int quantity)
        {
            currentHealth -= quantity;
            currentHealth = Mathf.Min(currentHealth, maxHealth); // reduce health to max if healed beyond maximum
        }

        void OnValidate()
        {
            maxHealth = Mathf.Max(1, maxHealth);
            defense = Mathf.Clamp(defense, 0, 100);
            minDamage = Mathf.Max(0, minDamage);
            maxDamage = Mathf.Max(minDamage, maxDamage);
        }
    } 
}