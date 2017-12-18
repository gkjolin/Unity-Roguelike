using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;

namespace AKSaigyouji.Roguelike
{
    // Having a kind of "utility" class with multiple responsibilities is generally not good practice. 
    // These formulas may be distributed to more appropriate places later, but for now they're lumped up here
    // for lack of a better place for them. All of the formulas are simple, in the sense that they do not have
    // dependencies. 

    /// <summary>
    /// A collection of simple formulas for converting raw stats into usable numbers.
    /// </summary>
    public static class StatFormulas
    {
        // some breakpoints for reference:
        // 50:    14%
        // 100:   25%
        // 300:   50%
        // 500:   63%
        // 1000:  77%
        // 3000:  91%
        // 10000: 97%
        const double K_ARMOR = 300;

        /// <summary>
        /// Compute the amount of damage reduction associated with this quantity of armor as a percentage, expressed
        /// as a float between 0 (no damage reduction) and 1 (100% damage reduction).
        /// </summary>
        /// <param name="armor">Must be at least 0.</param>
        public static float ComputeDamageReduction(int armor)
        {
            // We could choose to allow negative armor and have that correspond to damage vulnerability (with
            // the formula adjusted accordingly) but that could mask some bugs. Instead, damage vulnerability
            // will have to be implemented more explicitly if desired.
            if (armor < 0)
                throw new ArgumentOutOfRangeException("armor");

            return (float)PercentageWithDiminishingReturns(armor, K_ARMOR);
        }

        /// <summary>
        /// Returns a percentage (0 to 1) that scales with value. Uses a common formula for e.g. damage reduction
        /// relative to an uncapped value. An example is the armor formula for Diablo 3. 
        /// </summary>
        /// <param name="value">The value being converted to a percentage.</param>
        /// <param name="k">A hyper-parameter that will affect rate of growth inversely (higher k, slower growth).</param>
        static double PercentageWithDiminishingReturns(int value, double k)
        {
            /* 
             This is based on the standard formula 1 / (0.01 + k / value).
             As value grows large, k / value goes to 0, and the whole expresion goes to 100 (understood as a percentage).
             Since we want a ratio (0 to 1) we instead use (1 / (1 + k / value)) with k scaled accordingly, which is 
             equivalent to the following expression (faster to compute).
            */
            return value / (value + k);
        }
    } 
}