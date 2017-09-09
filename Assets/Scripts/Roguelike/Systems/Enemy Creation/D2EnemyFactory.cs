/* This is a factory class for all enemies, handling most of the wiring that needs to happen between components
 * at run-time. Instead of having each component on enemies use GetComponent/FindComponent/etc, this class injects
 * those dependencies into the components. This removes a lot of spaghetti logic from the components and makes
 * diagnosing dependency issues a lot easier.
 * 
 * Currently enemies are not pooled, and get destroyed when a level is complete. */

using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using AKSaigyouji.Maps;

namespace AKSaigyouji.Roguelike
{
    public sealed class D2EnemyFactory : GameBehaviour
    {
        [SerializeField] Transform player;
        [SerializeField] ItemFactory itemFactory;
        [SerializeField] Ground ground;
        [SerializeField] NameGenerator nameGenerator;
        [SerializeField] MapFilter mapFilter;

        IMap Map { get { return mapFilter.Map; } }

        ChampionBuff[] championBuffs = new ChampionBuff[]
        {
            new Berserker(), new Champion(), new Fanatic(), new Ghostly()
        };

        EliteBuff eliteBuff = new Elite();

        // for now, every enemy gets the same pathfinder. Each instance uses a decent chunk of storage internally,
        // so this saves a lot of memory but prevents multi-threaded AI.
        IPathFinder pathFinder = new DijkstraPathFinder();

        void Start()
        {
            Assert.IsNotNull(player);
            Assert.IsNotNull(itemFactory);
            Assert.IsNotNull(ground);
            Assert.IsNotNull(nameGenerator);
            Assert.IsNotNull(mapFilter);
        }

        public GameObject Build(Coord location, GameObject prefab)
        {
            return Instantiate(location, prefab, prefab.name);
        }

        public GameObject BuildEliteLeader(Coord location, GameObject prefab)
        {
            string name = nameGenerator.BuildName();
            GameObject enemy = Instantiate(location, prefab, name, eliteBuff);
            enemy.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 0.8f);
            return enemy;
        }

        public GameObject BuildEliteMinion(GameObject leader, Coord location, GameObject prefab)
        {
            // currently the minions have no special relationship to the leader, but in the future they should
            string name = "Minion " + prefab.name;
            GameObject enemy = Instantiate(location, prefab, name, eliteBuff);
            enemy.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 0.8f);
            return enemy;
        }
        
        public GameObject BuildChampion(Coord location, GameObject prefab)
        {
            ChampionBuff championBuff = championBuffs[UnityEngine.Random.Range(0, championBuffs.Length)];
            string name = string.Format("{0} {1}", championBuff.DisplayName, prefab.name);
            GameObject enemy = Instantiate(location, prefab, name, championBuff);
            enemy.GetComponent<SpriteRenderer>().color = new Color(135f / 255, 206f / 255, 235f / 255);
            return enemy;
        }

        GameObject Instantiate(Coord location, GameObject prefab, string name, IEnemyEnhancement enhancement = null)
        {
            GameObject enemyGO = Instantiate(prefab);
            enemyGO.name = name;
            enemyGO.transform.position = (Vector2)location;
            enemyGO.transform.parent = transform;

            var ai                = enemyGO.GetComponent<EnemyAI>();
            var body              = enemyGO.GetComponent<EnemyBody>();
            var enemyStats        = enemyGO.GetComponent<EnemyStats>();
            var itemsOnDeath      = enemyGO.GetComponent<OnDeathDropItems>();
            var experienceOnDeath = enemyGO.GetComponent<OnDeathExperience>();

            var playerStats = player.GetComponent<PlayerStats>();

            ai.Initialize(Map, player, enemyStats, pathFinder);
            body.Initialize(enemyStats);
            itemsOnDeath.Initialize(itemFactory, ground);
            experienceOnDeath.Initialize(playerStats);

            if (enhancement != null)
            {
                experienceOnDeath.Experience = enhancement.EnhanceExperience(experienceOnDeath.Experience);
                enemyStats.Promote(enhancement);
            }

            return enemyGO;
        }
    } 
}