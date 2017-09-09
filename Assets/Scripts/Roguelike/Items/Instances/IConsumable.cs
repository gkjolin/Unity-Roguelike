using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

namespace AKSaigyouji.Roguelike
{
    /// <summary>
    /// Interface for items that can be consumed.
    /// </summary>
    public interface IConsumable
    {
        /// <summary>
        /// Use up the item. The consumer represents the entity consuming the item. 
        /// </summary>
        void Use(GameObject consumer);
    }
}