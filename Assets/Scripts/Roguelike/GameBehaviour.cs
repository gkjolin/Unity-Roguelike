/* This class was designed to provide access to major game events (on level complete, on time change, etc.) without 
 * having to write boilerplate to subscribe/unsubscribe to those events. The one inconvenience of this approach
 * is that when making use of the OnEnable/OnDisable functions, an inheriter of this class must override them
 * and also make a call to base.OnEnable and base.OnDisable. Fortunately the compiler does issue a warning when writing
 * an OnEnable or OnDisable method without overriding.*/

using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

namespace AKSaigyouji.Roguelike
{
    /// <summary>
    /// MonoBehaviour augmented with event hooks.
    /// </summary>
    public abstract class GameBehaviour : MonoBehaviour
    {
        protected virtual void OnEnable()
        {
            GameTime.OnTimeChange += OnPlayerAction;
            MapBuilder.OnMapChange += OnNewMapGenerated;
            MapBuilder.OnMapExit += OnLevelComplete;
        }

        protected virtual void OnDisable()
        {
            GameTime.OnTimeChange -= OnPlayerAction;
            MapBuilder.OnMapChange -= OnNewMapGenerated;
            MapBuilder.OnMapExit -= OnLevelComplete;
        }

        /// <summary>
        /// Called whenever the player takes any kind of action. Note that this coincides with any and all changes to GameTime,
        /// i.e. the game time will only change when the player acts.
        /// </summary>
        protected virtual void OnPlayerAction()
        {

        }

        /// <summary>
        /// Called whenever the player leaves a level. Useful place to teardown level-specific content before new content
        /// is generated.
        /// </summary>
        protected virtual void OnLevelComplete()
        {

        }

        /// <summary>
        /// Called whenever a new map is created and populated with content.
        /// </summary>
        protected virtual void OnNewMapGenerated()
        {

        }
    } 
}