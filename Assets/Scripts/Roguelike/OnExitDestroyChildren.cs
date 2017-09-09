using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

namespace AKSaigyouji.Roguelike
{
    /// <summary>
    /// Destroys all children of this game object once the player exits a level.
    /// </summary>
    public sealed class OnExitDestroyChildren : GameBehaviour
    {
        protected override void OnLevelComplete()
        {
            foreach (Transform child in transform)
            {
                Destroy(child.gameObject);
            }
        }
    } 
}