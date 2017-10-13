using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

namespace AKSaigyouji.Roguelike
{
    /// <summary>
    /// Represents anything that can be attacked.
    /// </summary>
    public interface IAttackable
    {
        void Attack(int attackRoll, int damageRoll, string attackText);
        string Name { get; }
    } 
}