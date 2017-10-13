/* In D2 the standard champion variation offers the following benefits:
 * 2x damage
 * 33-50% increased elemental damage
 * attack rating +75%
 * attack rate +120%
 * velocity x2
 * +20% speed
 *
 * This implementation offers the 2x damage, a flat +10 to accuracy, +120% attack speed, and +100% move speed.*/

using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

namespace AKSaigyouji.Roguelike
{
    /// <summary>
    /// Standard version of the champion buff which improves the monster all-around.
    /// </summary>
    public sealed class Champion : ChampionBuff
    {
        public override string DisplayName { get { return "Champion"; } }

        public override int EnhanceDamage(int damage)
        {
            return base.EnhanceDamage(damage) + damage;
        }

        public override int EnhanceAccuracy(int accuracy)
        {
            return base.EnhanceAccuracy(accuracy) + 10;
        }

        public override float EnhanceMoveSpeed(float speed)
        {
            return base.EnhanceMoveSpeed(speed) / 2f;
        }

        public override float EnhanceAttackSpeed(float speed)
        {
            return base.EnhanceAttackSpeed(speed) / 2.2f;
        }
    } 
}