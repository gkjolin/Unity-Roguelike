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
        Defense GetDefense();
        void Attack(AttackResult result);
    } 
}