using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

namespace AKSaigyouji.Roguelike
{
    // Inheritance is the wrong tool for this job. Should find a way to do this via composition in the future.

    /// <summary>
    /// Behaves like the standard enemy body, but all attacks have a % chance to miss due to ethereality. 
    /// </summary>
    public sealed class EnemyBodyEthereal : EnemyBody
    {
        [Tooltip("Percent chance to miss, from 0 to 100.")]
        [SerializeField] int chanceToMiss = 50;

        public override void Attack(int damageRoll, string attackText)
        {
            if (UnityEngine.Random.Range(0, 100) > chanceToMiss)
            {
                base.Attack(damageRoll, attackText);
            }
            else
            {
                Logger.Log("Your attack phases through!");
            }
        }
    } 
}