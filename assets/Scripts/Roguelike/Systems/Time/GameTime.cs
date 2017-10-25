using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace AKSaigyouji.Roguelike
{
    /// <summary>
    /// Represents the in-game time. Used to determine when non-player characters and events will act.
    /// </summary>
    public sealed class GameTime : MonoBehaviour
    {
        internal delegate void TimeChangeEventHandler();

        /// <summary>
        /// Used internally - do not subscribe directly to this event.
        /// </summary>
        internal static event TimeChangeEventHandler OnTimeChange;

        public static double Current { get { return time; } }
        static double time = 0;

        public void Increase(double increment)
        {
            Assert.IsTrue(increment >= 0, "Attempted to increment time negatively.");
            time += increment;
            if (OnTimeChange != null)
            {
                OnTimeChange();
            }
        }
    } 
}