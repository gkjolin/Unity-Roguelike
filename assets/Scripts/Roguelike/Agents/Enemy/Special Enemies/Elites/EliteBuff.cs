using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

namespace AKSaigyouji.Roguelike
{
    public abstract class EliteBuff : IEnemyEnhancement
    {
        public int EnhanceExperience(int experience)
        {
            return 4 * experience;
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

        public virtual int EnhanceMaxHealth(int maxHealth)
        {
            return maxHealth;
        }

        public virtual float EnhanceMoveSpeed(float speed)
        {
            return speed;
        }
    }
}