using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Assertions;
using AKSaigyouji.Maps;

namespace AKSaigyouji.Roguelike
{
    public sealed class PlayerController : MonoBehaviour
    {
        [SerializeField] UnityEvent onLevelExit;
        [SerializeField] GameTime time;
        [SerializeField] PlayerStats stats;
        [SerializeField] PlayerAttack attack;
        [SerializeField] Inventory inventory;
        [SerializeField] MapFilter mapFilter;

        float MoveSpeed { get { return stats.Speed; } }
        float AttackSpeed { get { return 2 * stats.Speed; } }
        float PickupSpeed { get { return stats.Speed / 2; } }

        IMap Map { get { return mapFilter.Map; } }

        bool standingOnExit = false;

        void Start()
        {
            Assert.IsNotNull(time);
            Assert.IsNotNull(stats);
            Assert.IsNotNull(attack);
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
                time.Increase(PickupSpeed);
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
                        attack.Attack(target);
                        time.Increase(AttackSpeed);
                    }
                }
                else
                {
                    transform.position = (Vector2)coord;
                    time.Increase(MoveSpeed);
                }
            }
        }
    } 
}