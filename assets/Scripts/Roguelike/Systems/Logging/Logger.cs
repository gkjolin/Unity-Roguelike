/* Currently this simply delegates down to Debug.Log, but this is obviously something that would need to be replaced
 * with some kind of custom in-game window in the future, warranting a layer of indirection. */

using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

namespace AKSaigyouji.Roguelike
{
    /// <summary>
    /// Communicates various game events to the player.
    /// </summary>
    public sealed class Logger : MonoBehaviour
    {
        static Logger instance;

        public static void Log(string text)
        {
            Debug.LogFormat(text);
        }

        // Similarly to string.Format, several explicit overloads are provided to avoid creating the params array
        // in the final overload in cases where there are fewer than 4 arguments in the format string.

        public static void LogFormat(string text, object arg0)
        {
            Log(string.Format(text, arg0));
        }

        public static void LogFormat(string text, object arg0, object arg1)
        {
            Log(string.Format(text, arg0, arg1));
        }

        public static void LogFormat(string text, object arg0, object arg1, object arg2)
        {
            Log(string.Format(text, arg0, arg1, arg2));
        }

        public static void LogFormat(string text, params object[] args)
        {
            Log(string.Format(text, args));
        }
    } 
}