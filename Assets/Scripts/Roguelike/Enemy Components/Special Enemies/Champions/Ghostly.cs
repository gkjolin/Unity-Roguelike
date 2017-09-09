/* In D2, ghostly champions have the following bonuses:
 * half speed
 * 20% chance for cold damage
 * 50% physical resistance
 * 
 * This game does not (yet?) have physical resistance or cold damage, so we make the following changes instead:
 * -50% move speed, and +100% health */

using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

namespace AKSaigyouji.Roguelike
{
    public sealed class Ghostly : ChampionBuff
    {
        public override string DisplayName { get { return "Ghostly"; } }

        public override float EnhanceMoveSpeed(float speed)
        {
            return base.EnhanceMoveSpeed(speed) * 2;
        }

        public override int EnhanceMaxHealth(int maxHealth)
        {
            return base.EnhanceMaxHealth(maxHealth) * 2;
        }
    } 
}