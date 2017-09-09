/* This encapsulates a single enemy prefab, and contains information needed when spawning the enemy into the game.*/

using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

namespace AKSaigyouji.Roguelike
{
    [CreateAssetMenu(fileName = "Enemy", menuName = "AKSaigyouji/D2 Example/Enemy")]
    public sealed class D2Enemy : ScriptableObject
    {
        public GameObject Prefab { get { return prefab; } }
        [SerializeField] GameObject prefab;

        public GameObject MinionPrefab
        {
            get
            {
                // The null coalescing operator does not work properly on unity Objects. 
                if (minionPrefab == null) return prefab;
                return minionPrefab;
            }
        }
        [Tooltip("When spawning as a group/elite/champion, will be accompanied by a group of this type.")]
        [SerializeField] GameObject minionPrefab;

        [Tooltip("When spawning a group of normal versions of this enemy, this is the min number that will spawn.")]
        public int normalGroupSizeMin = 1;
        [Tooltip("When spawning a group of normal versions of this enemy, this is the max number that will spawn.")]
        public int normalGroupSizeMax = 5;

        [Tooltip("Min number of minions/followers in a champion or elite pack.")]
        public int numMinionsMin = 3;
        [Tooltip("Max number of minions/followers in a champion or elite pack.")]
        public int numMinionsMax = 4;

        public int GetRandomGroupSize(System.Random random)
        {
            return random.Next(normalGroupSizeMin, normalGroupSizeMax);
        }

        public int GetRandomMinionCount(System.Random random)
        {
            return random.Next(numMinionsMin, numMinionsMax);
        }
    } 
}