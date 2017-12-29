/* Player controls work in three layers: Input -> Controller -> Components. The controller provides a simplified
 * interface for player behaviour in the form of a set of commands. It has no knowledge on where these commands
 * are coming from, allowing e.g. to have input from a keyboard or from an AI, or even a memorized list (for replay
 * functionality).*/

using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Assertions;

namespace AKSaigyouji.Roguelike
{
    public sealed class PlayerController : MonoBehaviour
    {
        [SerializeField] UnityEvent onLevelExit;
        [SerializeField] GameTime time;
        [SerializeField] PlayerStats stats;
        [SerializeField] PlayerAttacker attacker;
        [SerializeField] AttackResolver attackResolver;
        [SerializeField] Inventory inventory;
        [SerializeField] MapFilter mapFilter;

        int MoveSpeed { get { return stats.GetAttribute(Attribute.MoveSpeed); } }
        int AttackSpeed { get { return stats.GetAttribute(Attribute.AttackSpeed); } }
        int PickupSpeed { get { return stats.GetAttribute(Attribute.PickupSpeed); } }

        IMap Map { get { return mapFilter.Map; } }

        bool standingOnExit = false;

        void Start()
        {
            Assert.IsNotNull(time);
            Assert.IsNotNull(stats);
            Assert.IsNotNull(attacker);
            Assert.IsNotNull(attackResolver);
            Assert.IsNotNull(mapFilter);
            Assert.IsNotNull(inventory);
        }

        public void MovePlayer(int x, int y)
        {
            Assert.IsTrue(Math.Abs(x) <= 1 && Math.Abs(y) <= 1);
            Coord newCoord = (Coord)transform.position + new Coord(x, y);
            TryMoveTo(newCoord);
        }

        public void TryMoveDownStairs()
        {
            if (standingOnExit)
            {
                onLevelExit.Invoke();
            }
        }

        public void TryPickupItem()
        {
            if (inventory.TryPickupItem())
            {
                time.IncreaseBasedOnSpeed(PickupSpeed);
            }
        }

        void OnTriggerEnter2D(Collider2D other)
        {
            if (other.tag == "Exit")
            {
                standingOnExit = true;
            }
        }

        void OnTriggerExit2D(Collider2D other)
        {
            if (other.tag == "Exit")
            {
                standingOnExit = false;
            }
        }

        void TryMoveTo(Coord coord)
        {
            if (Map.IsWalkable(coord))
            {
                Collider2D collision = CustomRaycast.GetMapEntityColliderAt(coord);
                if (collision != null) // something is in the way
                {
                    IAttackable target = collision.GetComponent<IAttackable>();
                    if (target != null) // that something can be attacked
                    {
                        Defense defense = target.GetDefense();
                        Attack attack = attacker.BuildAttack();
                        AttackResult result = attackResolver.ResolvePhysicalAttack(attack, defense);
                        target.Attack(result);
                        time.IncreaseBasedOnSpeed(AttackSpeed);
                    }
                }
                else
                {
                    transform.position = (Vector2)coord;
                    time.IncreaseBasedOnSpeed(MoveSpeed);
                }
            }
        }
    } 
}