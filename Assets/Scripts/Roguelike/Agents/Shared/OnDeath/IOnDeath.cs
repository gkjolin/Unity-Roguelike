using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

namespace AKSaigyouji.Roguelike
{
    /// <summary>
    /// Represents a behaviour that does something upon the death of its host.
    /// </summary>
    public interface IOnDeath
    {
        void Invoke();
    } 
}