/* The main motivation for this class was to avoid having to deal with layermask logic in every class that needed to
 * use raycasts. By wrapping some of unity's Physics2D functions we can remove layermask logic from the raycasting
 * interface so that this is the only class worrying about layermasks. */

using UnityEngine;

namespace AKSaigyouji.Roguelike
{
    /// <summary>
    /// Provides custom raycast logic, serving as a thin wrapper over Physics2D raycasting functionality.
    /// </summary>
    public static class CustomRaycast
    {
        static int mapEntityLayerMask;
        static int itemLayerMask;

        static CustomRaycast()
        {
            mapEntityLayerMask = 1 << LayerMask.NameToLayer("MapEntity");
            itemLayerMask = 1 << LayerMask.NameToLayer("Item");
        }

        /// <summary>
        /// Gets the collider of the map entity at these coordinates if it exists, based on the chosen layer mask. 
        /// Returns null otherwise.
        /// </summary>
        public static Collider2D GetMapEntityColliderAt(Vector2 coordinates)
        {
            return Physics2D.OverlapPoint(coordinates, mapEntityLayerMask);
        }

        /// <summary>
        /// Gets the collider of the item at these coordinates if it exists, based on the chosen layer mask. 
        /// Returns null otherwise.
        /// </summary>
        public static Collider2D GetItemColliderAt(Vector2 coordinates)
        {
            return Physics2D.OverlapPoint(coordinates, itemLayerMask);
        }
    } 
}