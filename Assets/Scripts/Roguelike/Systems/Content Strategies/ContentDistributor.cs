using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using AKSaigyouji.Maps;
using AKSaigyouji.AtlasGeneration;

namespace AKSaigyouji.Roguelike
{
    /// <summary>
    /// Responsible for assigning locations in an atlas for content placement.
    /// </summary>
    public sealed class ContentDistributor
    {
        readonly Map map;
        readonly System.Random random;
        readonly Chart[] charts;
        readonly Vector2 startLocation;

        public ContentDistributor(Map map, IEnumerable<Chart> charts, Vector3 startLocation, int seed)
        {
            if (map == null) throw new ArgumentNullException("map");
            if (charts == null) throw new ArgumentNullException("charts");
            if (charts.Any(ch => ch == null)) throw new ArgumentException("One or more charts is null.");
            random = new System.Random(seed);
            this.map = map;
            this.charts = charts.ToArray();
            this.startLocation = startLocation;
        }

        /// <summary>
        /// Randomly gathers a collection of locations for content to be placed, based on a uniform grid of cells. Each
        /// cell is a square of map tiles, and can hold at most 1 location. 
        /// </summary>
        /// <param name="cellSize">The size of each cell in map tiles. e.g. a size of 3 will chop the map into blocks
        /// of 3 by 3. Must be at least 1.</param>
        /// <param name="contentDensity">The probability that a given tile will be chosen as a content location, subject
        /// to the constraint that only 1 tile per cell can be chosen. Must be a float between 0 and 1 (inclusive).</param>
        /// <param name="minDistanceFromStart">A tile must be at least this far away from the start to be chosen. Must be
        /// at least 0. A value of 0 results in no restriction on where tiles can be placed.</param>
        public List<Coord> DetermineLocations(int cellSize, float contentDensity, float minDistanceFromStart = 0)
        {
            if (minDistanceFromStart < 0)
                throw new ArgumentOutOfRangeException("minDistanceFromStart", "Must be nonnegative.");

            if (cellSize <= 0)
                throw new ArgumentOutOfRangeException("cellSize", "Must be at least 1.");

            if (1 < contentDensity || 0 > contentDensity)
                throw new ArgumentOutOfRangeException("contentDensity", "Must be between 0 and 1 inclusive.");

            var spacePartition = new SpacePartition(cellSize, map.Length, map.Width);
            var contentLocations = new List<Coord>();
            foreach (var chart in charts)
            {
                Map map = chart.Map;
                map.ForEach(predicate: map.IsFloor, action: (x, y) =>
                {
                    x += chart.Offset.x;
                    y += chart.Offset.y;
                    if (random.NextDouble() < contentDensity)
                    {
                        Coord location = new Coord(x, y);
                        float distanceFromEntrance = Vector2.Distance(startLocation, location);
                        if (!spacePartition.IsAdjacentToObject(x, y) && distanceFromEntrance >= minDistanceFromStart)
                        {
                            contentLocations.Add(location);
                            spacePartition.PlaceObject(x, y);
                        }
                    }
                });
            }
            return contentLocations;
        }
    } 
}