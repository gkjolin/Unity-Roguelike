using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using AKSaigyouji.Maps;

namespace AKSaigyouji.Roguelike
{
    public abstract class EnemyAI : GameBehaviour
    {
        protected IMap Map { get { return map; } }
        protected EnemyStats Stats { get { return stats; } }

        // enemies are created dynamically, so we can't manually set up dependencies in the editor (and there would
        // be too many anyway). Instead, these have to be set up with the Initialize method.
        IPathFinder pathFinder;
        Transform target;
        EnemyStats stats;
        IMap map;
        double timeToNextAction;

        void Start()
        {
            timeToNextAction = GameTime.Current;
        }

        public void Initialize(IMap map, Transform target, EnemyStats stats, IPathFinder pathFinder)
        {
            if (map == null) throw new ArgumentNullException("map");
            if (target == null) throw new ArgumentNullException("target");
            if (stats == null) throw new ArgumentNullException("stats");
            if (pathFinder == null) throw new ArgumentNullException("pathFinder");

            this.map = map;
            this.target = target;
            this.stats = stats;
            this.pathFinder = pathFinder;
        }

        protected abstract double Act(Transform target);

        /// <summary>
        /// Performs a search to find a path between start and end, of total length
        /// not exceeding the given max distance. Uses the passed in list to populate the path, replacing
        /// the list's existing contents. Returns true if a valid path is found, otherwise returns false.
        /// </summary>
        protected bool FindPath(Coord start, Coord end, int maxDistance, List<Coord> path)
        {
            return pathFinder.FindPath(map, start, end, maxDistance, path);
        }

        protected bool CanMoveTo(Coord position)
        {
            return map.IsWalkable(position) && CustomRaycast.GetMapEntityColliderAt(position) == null;
        }

        protected override void OnPlayerAction()
        {
            while (IsTimeToAct)
            {
                double timeSpent = Act(target);
                if (timeSpent <= 0)
                {
                    throw new InvalidOperationException("Infinite loop averted: AI must spend (positive) time on each action.");
                }
                timeToNextAction += timeSpent;
            }
        }

        bool IsTimeToAct { get { return GameTime.Current >= timeToNextAction; } }
    }  
}