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

        // a speed of 50 corresponds to 6 seconds, the 'standard' move speed.
        const double SPEED_TO_TIME_COEFFICIENT = 300;

        public void Increase(double increment)
        {
            Assert.IsTrue(increment >= 0, "Attempted to increment time negatively.");
            time += increment;
            OnTimeChange?.Invoke();
        }

        /// <summary>
        /// Shortcut for computing time from speed and increasing by that amount.
        /// </summary
        public void IncreaseBasedOnSpeed(int speed)
        {
            Increase(ComputeTimeTaken(speed));
        }

        /// <summary>
        /// Computes the time taken by an action of the given speed. i.e. converts speed to time. Faster speed,
        /// less time taken.
        /// </summary>
        public static double ComputeTimeTaken(int speed)
        {
            if (speed < 1)
                throw new ArgumentOutOfRangeException("speed", "Must be at least 1.");

            return SPEED_TO_TIME_COEFFICIENT / speed;
        }
    } 
}