using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using AKSaigyouji.Maps;

namespace AKSaigyouji.Roguelike
{
    /// <summary>
    /// Holds a map, analogous to the relationship between MeshFilters and Meshes.
    /// </summary>
    public sealed class MapFilter : MonoBehaviour
    {
        public IMap Map { get { return map; } set { map = value; } }
        IMap map;
    }   
}