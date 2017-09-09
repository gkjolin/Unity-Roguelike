using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using AKSaigyouji.Maps;

namespace AKSaigyouji.Roguelike
{
    /// <summary>
    /// Responsible for assigning encounters on a map based on location.
    /// </summary>
    public sealed class EncounterBuilder
    {
        readonly System.Random random;
        readonly Map map;
        readonly D2EnemyFactory factory;
        readonly HashSet<Coord> occupied;
        // the point of this array is to be able to iterate over the 20 closest points around a 
        // coordinate, plus the coordinate itself, in order of distance from the coordinate.
        readonly Coord[] offsets = new[]
        {
            Coord.zero,
            new Coord(-1, 0), new Coord(1, 0), new Coord(0, 1), new Coord(0, -1),
            new Coord(-1, -1), new Coord(1, 1), new Coord(-1, 1), new Coord(1, -1),
            new Coord(2, 0), new Coord(-2, 0), new Coord(0, 2), new Coord(0, -2),
            new Coord(2, 1), new Coord(2, -1), new Coord(-2, 1), new Coord(2, 1),
            new Coord(1, 2), new Coord(1, -2), new Coord(-1, 2), new Coord(-1, -2)
        };

        public EncounterBuilder(Map globalMap, D2EnemyFactory factory, HashSet<Coord> occupied, int seed)
        {
            map = globalMap;
            random = new System.Random(seed);
            this.factory = factory;
            this.occupied = occupied;
        }

        public void PlaceEncounters(IEnumerable<Coord> locations, D2Enemy[] enemies, float champProb, float eliteProb)
        {
            var encounterLocations = new List<Coord>(offsets.Length);

            foreach (Coord position in locations)
            {
                encounterLocations.Clear();

                D2Enemy enemy = enemies[random.Next(0, enemies.Length)];
                double enemyTypeRoll = random.NextDouble();
                int numEnemies;
                D2EnemyType type;
                if (enemyTypeRoll < eliteProb)
                {
                    numEnemies = enemy.GetRandomMinionCount(random);
                    type = D2EnemyType.Elite;
                }
                else if (enemyTypeRoll < champProb + eliteProb)
                {
                    numEnemies = enemy.GetRandomMinionCount(random);
                    type = D2EnemyType.Champion;
                }
                else
                {
                    numEnemies = enemy.GetRandomGroupSize(random);
                    type = D2EnemyType.Basic;
                }

                foreach (Coord offset in offsets)
                {
                    Coord coord = position + offset;
                    if (!occupied.Contains(coord) && !map.IsWallOrVoid(coord))
                    {
                        encounterLocations.Add(coord);
                    }
                }

                if (encounterLocations.Count > 0)
                {
                    encounterLocations = encounterLocations.Take(numEnemies).ToList();
                    foreach (Coord coord in encounterLocations)
                    {
                        occupied.Add(coord);
                    }
                    CreateGroup(encounterLocations, enemy, type);
                }
            }
        }

        void CreateGroup(IEnumerable<Coord> locations, D2Enemy enemy, D2EnemyType enemyType)
        {
            Assert.IsNotNull(locations);
            Assert.IsNotNull(enemy);
            Assert.IsTrue(locations.Count() > 0);
            switch (enemyType)
            {
                case D2EnemyType.Basic:
                    CreateBasicGroup(locations, enemy);
                    break;
                case D2EnemyType.Champion:
                    CreateChampionGroup(locations, enemy);
                    break;
                case D2EnemyType.Elite:
                    CreateEliteGroup(locations, enemy);
                    break;
                default:
                    throw new System.ComponentModel.InvalidEnumArgumentException();
            }
        }

        void CreateBasicGroup(IEnumerable<Coord> locations, D2Enemy enemy)
        {
            foreach (Coord coord in locations)
            {
                factory.Build(coord, enemy.Prefab);
            }
        }

        void CreateEliteGroup(IEnumerable<Coord> locations, D2Enemy enemy)
        {
            Coord leaderLocation = locations.First();
            GameObject leader = factory.BuildEliteLeader(leaderLocation, enemy.Prefab);
            foreach (Coord coord in locations.Skip(1))
            {
                factory.BuildEliteMinion(leader, coord, enemy.MinionPrefab);
            }
        }

        void CreateChampionGroup(IEnumerable<Coord> locations, D2Enemy enemy)
        {
            foreach (Coord coord in locations)
            {
                factory.BuildChampion(coord, enemy.Prefab);
            }
        }
    } 
}