using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

namespace AKSaigyouji.Roguelike
{
    public interface IHealable
    {
        void Heal(int amount);
    } 
}