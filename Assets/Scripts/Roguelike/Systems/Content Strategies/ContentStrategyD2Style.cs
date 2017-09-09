using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using AKSaigyouji.Maps;
using AKSaigyouji.AtlasGeneration;

namespace AKSaigyouji.Roguelike
{
    public sealed class ContentStrategyD2Style : ContentStrategy
    {
        [SerializeField] int cellSize = 3;
        [SerializeField] float contentDensity = 0.1f;
        [SerializeField] float promotionRate = 0.05f;

        [Tooltip("Enemies will not spawn within this distance of the entrance.")]
        [SerializeField] float minDistanceFromEntrance = 5;

        [SerializeField] D2Enemy[] enemies;
        [SerializeField] D2EnemyFactory enemyFactory;
        [SerializeField] Ground ground;
        [SerializeField] Transform environment; // used to organize environment objects 

        [SerializeField] int seed = 0;

        public int Seed { get { return seed; } set { seed = value; } }

        public override void GenerateContent(Atlas atlas)
        {
            Map globalMap = atlas.GlobalMap;
            Coord start = GetStartPosition(atlas);
            var occupied = new HashSet<Coord>() { start };
            var encounterBuilder = new EncounterBuilder(globalMap, enemyFactory, occupied, seed);
            var contentDistributor = new ContentDistributor(globalMap, atlas.Charts, start, seed);
            IEnumerable<Coord> enemyLocations = contentDistributor.DetermineLocations(cellSize, contentDensity, minDistanceFromEntrance);
            encounterBuilder.PlaceEncounters(enemyLocations, enemies, promotionRate, promotionRate);
        }
    
        void OnValidate()
        {
            cellSize = Mathf.Max(1, cellSize);
            contentDensity = Mathf.Clamp(contentDensity, 0f, 1f);
        }

        Coord GetStartPosition(Atlas atlas)
        {
            return (Coord)Atlas.EnumerateMarkers(atlas.Charts)
                               .First(marker => marker.Filter("_start"))
                               .GlobalPositon;
        }
    }
}