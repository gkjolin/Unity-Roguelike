using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using AKSaigyouji.AtlasGeneration;

namespace AKSaigyouji.Roguelike
{
    /// <summary>
    /// Base class for a monobehaviour capable of generating content for an atlas.
    /// </summary>
    public abstract class ContentStrategy : MonoBehaviour
    {
        /// <summary>
        /// Generates content for the given atlas. In general, one content strategy need not handle the entire
        /// atlas. If using multiple strategies, then as a best practice used markers should be marked as Used.
        /// </summary>
        public abstract void GenerateContent(Atlas atlas);
    } 
}