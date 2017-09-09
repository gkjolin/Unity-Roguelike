using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using AKSaigyouji.AtlasGeneration;
using AKSaigyouji.Modules.MapGeneration;

namespace AKSaigyouji.Roguelike
{
    public sealed class MapBuilder : MonoBehaviour
    {
        public delegate void MapEventHandler();

        /// <summary>
        /// Used internally, do not subscribe directly to this event. Instead, make use of the corresponding event
        /// provided by the GameBehaviour class.
        /// </summary>
        internal static event MapEventHandler OnMapExit;

        /// <summary>
        /// Used internally, do not subscribe directly to this event. Instead, make use of the corresponding
        /// event provided by the GameBehaviour class.
        /// </summary>
        internal static event MapEventHandler OnMapChange;

        [SerializeField] MazeAtlasGenerator atlasGenerator;
        [SerializeField] TileGeneratorMS tileGenerator;
        [SerializeField] PlayerController player;
        [SerializeField] ContentStrategyD2Style contentStrategy;
        [SerializeField] MapFilter mapFilter;

        void Start()
        {
            GenerateNewMap();
        }

        public void ExitMap()
        {
            if (OnMapExit != null)
            {
                OnMapExit();
            }
            GenerateNewMap();
        }

        public void GenerateNewMap()
        {
            int seed = UnityEngine.Random.Range(int.MinValue, int.MaxValue);
            Atlas atlas = atlasGenerator.Generate(seed);
            mapFilter.Map = atlas.GlobalMap.ToIMap();
            tileGenerator.Generate(atlas.GlobalMap, mask: atlas.IsContainedInAtlas);
            player.transform.position = GetPlayerStartPosition(atlas);
            GenerateContent(atlas);
            if (OnMapChange != null)
            {
                OnMapChange();
            }
        }

        void GenerateContent(Atlas atlas)
        {
            foreach (var strategy in GetComponentsInChildren<ContentStrategy>())
            {
                strategy.GenerateContent(atlas);
            }
        }

        Coord GetPlayerStartPosition(Atlas atlas)
        {
            var startChart = Atlas.FilterByMetaData(atlas.Charts, "_start").FirstOrDefault();
            Assert.IsNotNull(startChart, "No chart marked '_start'");
            Marker startMarker = startChart.Markers.First(marker => marker.Filter("_start"));
            return (Coord)startMarker.GlobalPositon;
        }
    } 
}