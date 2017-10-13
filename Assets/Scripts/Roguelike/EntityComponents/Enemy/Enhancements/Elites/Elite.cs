/* Elite (unique) monsters in diablo 2 use a system of random bonuses (extra strong, extra fast, cold enchanted, fire
 * enchanted, aura enchanted, spectral hit, multishot, etc. Currently the infrastructure for such bonuses is lacking, 
 * so instead all elites simply get a buff to most of their stats. */

using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

namespace AKSaigyouji.Roguelike
{
    public sealed class Elite : EliteBuff
    {
        public override float EnhanceAttackSpeed(float speed)
        {
            return base.EnhanceAttackSpeed(speed) / 2;
        }

        public override int EnhanceAccuracy(int accuracy)
        {
            return base.EnhanceAccuracy(accuracy) + 10;
        }

        public override int EnhanceDamage(int damage)
        {
            return base.EnhanceDamage(damage) + damage / 2;
        }

        public override int EnhanceMaxHealth(int maxHealth)
        {
            return base.EnhanceMaxHealth(maxHealth) + maxHealth;
        }

        public override int EnhanceDefense(int defense)
        {
            return base.EnhanceDefense(defense) + defense / 2;
        }
    } 
}