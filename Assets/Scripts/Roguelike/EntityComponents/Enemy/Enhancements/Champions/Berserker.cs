/* D2 berserker champions have 4x damage and attack rating, and 1.5x hit points. Given how accuracy works in 
 our implementation, we add a flat bonus of 25 to accuracy.*/

using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

namespace AKSaigyouji.Roguelike
{
    public sealed class Berserker : ChampionBuff
    {
        public override string DisplayName { get { return "Berserker"; } }

        public override int EnhanceDamage(int damage)
        {
            return base.EnhanceDamage(damage) + 3 * damage;
        }

        public override int EnhanceAccuracy(int accuracy)
        {
            return base.EnhanceAccuracy(accuracy) + 25;
        }

        public override int EnhanceMaxHealth(int maxHealth)
        {
            return base.EnhanceMaxHealth(maxHealth) + maxHealth / 2;
        }
    } 
}