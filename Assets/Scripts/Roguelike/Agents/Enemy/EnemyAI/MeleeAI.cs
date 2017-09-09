using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using AKSaigyouji.Maps;

namespace AKSaigyouji.Roguelike
{ 
    public sealed class MeleeAI : EnemyAI
    {
        public int HostileRange { get { return hostileRange; } set { hostileRange = value; } }

        [Tooltip("Will pursue the target as long as it remains in this range.")]
        [SerializeField] int hostileRange = 10;
       
        [SerializeField] float moveSpeed = 6;
        [SerializeField] float attackSpeed = 12;

        float IdleSpeed { get { return moveSpeed / 2; } } // idling should be relatively fast.

        readonly List<Coord> path = new List<Coord>();
        Coord lastTargetCoords = new Coord(-1, -1); // Used to avoid recalculating path if target hasn't moved.

        readonly Coord INVALID_POSITION = new Coord(-1, -1);

        protected override double Act(Transform target)
        {
            Coord targetCoords = (Coord)target.position;
            if (IsTargetInAggroRange(targetCoords))
            {
                if (targetCoords != lastTargetCoords)
                {
                    UpdatePathTo(targetCoords);
                }
                if (IsPathValid) 
                {
                    if (NextMove == targetCoords)
                    {
                        Attack(target);
                        return attackSpeed;
                    }
                    else
                    {
                        bool moved = TryMoveTo(NextMove);
                        return moved ? moveSpeed : IdleSpeed;
                    }
                }
                else // if we don't have a path to the player, idle
                {
                    Idle();
                    return IdleSpeed;
                }
            }
            else // if player is not in range, idle
            {
                Idle();
                return IdleSpeed;
            }
        }

        // The path should only be empty if we have yet to calculate it, or if we calculated it and no path 
        // to the target was found. 
        bool IsPathValid { get { return path.Count > 0; } }

        // Note: we can only get the next move if the path is valid
        Coord NextMove { get { return path[path.Count - 1]; } }

        void UpdatePathTo(Coord target)
        {
            FindPath((Coord)transform.position, target, hostileRange, path);
            lastTargetCoords = target;
        }

        bool IsTargetInAggroRange(Vector2 targetPosition)
        {
            return Vector2.Distance(transform.position, targetPosition) < hostileRange;
        }

        /// <summary>
        /// Should be called when the current path is no longer valid for any reason.
        /// </summary>
        void InvalidatePath()
        {
            lastTargetCoords = INVALID_POSITION;
        }

        /// <summary>
        /// Increments the time until next action and invalidates the player position
        /// </summary>
        void Idle()
        {
            lastTargetCoords = INVALID_POSITION;
        }

        void Attack(Transform target)
        {
            IAttackable targetBody = target.GetComponent<IAttackable>();
            int accuracy = UnityEngine.Random.Range(0, 100) + Stats.Accuracy;
            int damage = UnityEngine.Random.Range(Stats.MinDamage, Stats.MaxDamage);
            targetBody.Attack(accuracy, damage, name + " attacks.");
        }

        bool TryMoveTo(Coord destination)
        {
            // This assertion is very useful in catching subtle bugs involving invalid path state.
            Assert.IsTrue(destination.SquaredDistance((Coord)transform.position) <= 2);
            if (CanMoveTo(destination))
            {
                MoveTo(destination);
                return true;
            }
            else
            {
                // The following will try to move to the two squares that are adjacent to both the destination 
                // and the current enemy position. If both are occupied, it will waste some time.
                Coord position = (Coord)transform.position;
                Coord alternatePositionA;
                Coord alternatePositionB;
                if (destination.y == position.y) // destination is horizontally adjacent to enemy position
                {
                    alternatePositionA = new Coord(destination.x, destination.y - 1);
                    alternatePositionB = new Coord(destination.x, destination.y + 1);
                }
                else if (destination.x == position.x) // destination is vertically adjacent to enemy position
                {
                    alternatePositionA = new Coord(destination.x - 1, destination.y);
                    alternatePositionB = new Coord(destination.x + 1, destination.y);
                }
                else // destination is diagonally adjacent to enemy position
                {
                    alternatePositionA = new Coord(destination.x, position.y);
                    alternatePositionB = new Coord(position.x, destination.y);
                }
                // If either alternate path is taken, it means we've stepped off the path, so we need 
                // to invalidate it. This ensures it will be recalculated next move.
                if (CanMoveTo(alternatePositionA))
                {
                    MoveTo(alternatePositionA);
                    InvalidatePath();
                    return true;
                }
                else if (CanMoveTo(alternatePositionB))
                {
                    MoveTo(alternatePositionB);
                    InvalidatePath();
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        void MoveTo(Coord position)
        {
            transform.position = (Vector2)position;
            path.RemoveAt(path.Count - 1);
        }
    } 
}