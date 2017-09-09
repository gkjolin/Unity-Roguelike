using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using AKSaigyouji.AtlasGeneration;

namespace AKSaigyouji.Roguelike
{
    /// <summary>
    /// A simple content strategy that instantiates a specific prefab for specific types of marker presets.
    /// </summary>
    public sealed class MarkerObjectSpawner : ContentStrategy
    {
        [Tooltip("Preset-Prefab pairs. Will instantiate associated prefab for each marker with given preset.")]
        [SerializeField] StringPrefabPair[] prefabs;

        [Tooltip("Instantiated objects will be children of this object.")]
        [SerializeField] Transform parent;

        [Tooltip("Any chart with a meta data key in this list will be skipped.")]
        [SerializeField] string[] chartsToSkip;

        public override void GenerateContent(Atlas atlas)
        {
            var prefabTable = prefabs.ToDictionary(pair => pair.Key, pair => pair.Value);
            var markers = atlas.Charts
                               .Where(chart => chartsToSkip.All(s => !chart.Filter(s)))
                               .SelectMany(chart => chart.UnusedMarkers)
                               .Where(marker => prefabTable.ContainsKey(marker.Preset));

            foreach (Marker marker in markers)
            {
                GameObject prefab = prefabTable[marker.Preset];
                GameObject createdObject = Instantiate(prefab, parent);
                createdObject.transform.position = (Coord)marker.GlobalPositon;
                marker.Use();
            }
        }
    }
}