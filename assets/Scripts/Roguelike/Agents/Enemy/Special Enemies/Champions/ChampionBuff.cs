/* All champions in D2 have 3x (2.5x in nightmare, 2x in hell) health and give 3x experience. They also have +2 to monster
 * level, though there is currently no equivalent concept in this game.*/

namespace AKSaigyouji.Roguelike
{
    /// <summary>
    /// A collection of enhancements representing the benefits a D2-style champion receives. Subtypes should enhance
    /// the base class' existing enhancements rather than replace them.
    /// </summary>
    public abstract class ChampionBuff : IEnemyEnhancement
    {
        public abstract string DisplayName { get; }

        public int EnhanceExperience(int experience)
        {
            return 3 * experience;
        }

        public virtual int EnhanceMaxHealth(int maxHealth)
        {
            return 3 * maxHealth;
        }

        public virtual int EnhanceAccuracy(int accuracy)
        {
            return accuracy;
        }

        public virtual float EnhanceAttackSpeed(float speed)
        {
            return speed;
        }

        public virtual int EnhanceDamage(int damage)
        {
            return damage;
        }

        public virtual int EnhanceDefense(int defense)
        {
            return defense;
        }

        public virtual float EnhanceMoveSpeed(float speed)
        {
            return speed;
        }
    }
}