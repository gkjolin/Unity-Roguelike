using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace AKSaigyouji.Roguelike
{ 
    public sealed class MeleeAI : EnemyAI
    {
        public int HostileRange { get { return hostileRange; } set { hostileRange = value; } }

        [Tooltip("Will pursue the target as long as it remains in this range.")]
        [SerializeField] int hostileRange = 10;
       
        [SerializeField] int moveSpeed = 40;
        [SerializeField] int attackSpeed = 25;

        int IdleSpeed { get { return moveSpeed * 2; } } // idling should be relatively fast.

        readonly List<Coord> path = new List<Coord>();
        Coord? lastTargetCoords = null;

        // The path should only be empty if we have yet to calculate it, or if we calculated it and no path 
        // to the target was found. 
        bool FoundPathToTarget { get { return path.Count > 0; } }

        Coord NextMove
        {
            get
            {
                if (!FoundPathToTarget) throw new InvalidOperationException("Can't extract next move without path.");
                return path[path.Count - 1];
            }
        }

        protected override double Act(Transform target)
        {
            Coord targetCoords = (Coord)target.position;
            int speedOfChosenAction;
            if (IsTargetInAggroRange(targetCoords))
            {
                if (HasTargetMoved(targetCoords))
                {
                    UpdatePathTo(targetCoords);
                }
                if (FoundPathToTarget) 
                {
                    if (NextMove == targetCoords)
                    {
                        Attack(target);
                        speedOfChosenAction = attackSpeed;
                    }
                    else
                    {
                        bool moved = TryMoveTo(NextMove);
                        speedOfChosenAction = moved ? moveSpeed : IdleSpeed;
                    }
                }
                else // if we don't have a path to the target, idle
                {
                    Idle();
                    speedOfChosenAction = IdleSpeed;
                }
            }
            else // if target is not in range, idle
            {
                Idle();
                speedOfChosenAction = IdleSpeed;
            }
            return GameTime.ComputeTimeTaken(speedOfChosenAction);
        }

        bool HasTargetMoved(Coord target)
        {
            return !lastTargetCoords.HasValue || lastTargetCoords.Value != target;
        }

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
            lastTargetCoords = null;
        }

        void Idle()
        {
            InvalidatePath();
        }

        void Attack(Transform target)
        {
            IAttackable targetBody = target.GetComponent<IAttackable>();
            int damage = UnityEngine.Random.Range(Stats.MinDamage, Stats.MaxDamage);
            targetBody.Attack(damage, name + " attacks.");
        }

        bool TryMoveTo(Coord destination)
        {
            // This assertion is useful in catching subtle bugs involving invalid path state.
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
                    alternatePositionA = destination.DownShift;
                    alternatePositionB = destination.UpShift;
                }
                else if (destination.x == position.x) // destination is vertically adjacent to enemy position
                {
                    alternatePositionA = destination.LeftShift;
                    alternatePositionB = destination.RightShift;
                }
                else // destination is diagonally adjacent to enemy position
                {
                    // there are four cases, but they're all handled by using the correct player coordinate
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