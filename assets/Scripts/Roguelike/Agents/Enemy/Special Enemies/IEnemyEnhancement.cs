using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

namespace AKSaigyouji.Roguelike
{
    /// <summary>
    /// Represents an enhanced enemy type, e.g. champion, elite, etc.
    /// </summary>
    public interface IEnemyEnhancement
    {
        // Perhaps a better design would be to have two abstractions: one for an enhanced enemy type, and one for
        // an individual enhancement. The enhanced enemy type would return a collection of individual enhancements. 
        // This way we would have two simple interfaces with one method each, though this also requires several 
        // new types to deal with the added flexibility. A worthwhile switch if we need to create a lot of enemy stats,
        // or if we wish to compose/decorate/adapt this interface a lot. 
        int EnhanceMaxHealth(int maxHealth);
        int EnhanceDefense(int defense);
        int EnhanceAccuracy(int accuracy);
        int EnhanceDamage(int damage);
        float EnhanceMoveSpeed(float speed);
        float EnhanceAttackSpeed(float speed);
        int EnhanceExperience(int experience);
    }
}