using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using AKSaigyouji.Maps;

namespace AKSaigyouji.Roguelike
{
    public sealed class DijkstraPathFinder : IPathFinder
    {
        readonly Queue<QNode> q = new Queue<QNode>();
        readonly HashSet<Coord> visited = new HashSet<Coord>();
        readonly Dictionary<Coord, Coord> parent = new Dictionary<Coord, Coord>();

        public bool FindPath(IMap map, Coord start, Coord end, int maxDistance, List<Coord> path)
        {
            q.Clear();
            visited.Clear();
            parent.Clear();
            path.Clear();

            q.Enqueue(new QNode(0, start));
            visited.Add(start);
            while (q.Count > 0)
            {
                QNode node = q.Dequeue();
                Coord currentCoord = node.position;
                float currentDistance = node.distance;

                if (currentCoord == end) // Search success: found a path.
                {
                    RecoverPath(start, end, path);
                    return true;
                }

                if (currentDistance > maxDistance) // Search failed: no path within max search distance.
                {
                    return false;
                }

                foreach (Coord neighbour in currentCoord.GetEightNeighbours())
                {
                    if (map.IsWalkable(neighbour) && !visited.Contains(neighbour))
                    {
                        parent[neighbour] = currentCoord;
                        q.Enqueue(new QNode(currentDistance + neighbour.Distance(currentCoord), neighbour));
                        visited.Add(neighbour);
                    }
                }
            }
            return false; // Search failed: no path to target exists.
        }

        void RecoverPath(Coord start, Coord end, List<Coord> path)
        {
            Coord current = end;
            while (current != start)
            {
                path.Add(current);
                current = parent[current];
            }
        }

        struct QNode
        {
            public readonly float distance;
            public readonly Coord position;

            public QNode(float distance, Coord position)
            {
                this.distance = distance;
                this.position = position;
            }
        }
    } 
}