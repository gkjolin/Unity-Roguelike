using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using AKSaigyouji.Maps;

namespace AKSaigyouji.Roguelike
{
    /// <summary>
    /// Provides an interface to the ground for the purposes of interacting with dropped items.
    /// </summary>
    public sealed class Ground : GameBehaviour
    {
        [SerializeField] GameObject itemPrefab;
        [SerializeField] MapFilter mapFilter;

        IMap Map { get { return mapFilter.Map; } }
        Stack<GameObject> pooledGameObjects = new Stack<GameObject>();

        // useful diagnostic info
        [SerializeField, ReadOnly] int numItemsOnGround = 0;

        void Start()
        {
            Assert.IsNotNull(itemPrefab);
            Assert.IsNotNull(mapFilter);
        }

        /// <summary>
        /// Obtains a reference to the item on the ground at this location. Returns null if no item is found.
        /// </summary>
        public Item GetItemAt(Vector2 location)
        {
            ItemFilter itemFilter = GetItemFilter(location);
            if (itemFilter != null)
            {
                return itemFilter.Item;
            }
            return null;
        }

        /// <summary>
        /// Removes the item from the ground at this location, if there is one.
        /// </summary>
        /// <returns>Boolean indicating whether an item was successfully removed from this location.</returns>
        public bool RemoveItem(Vector2 location)
        {
            ItemFilter itemFilter = GetItemFilter(location);
            Assert.IsNotNull(itemFilter, "Found item without an item filter component.");
            if (itemFilter != null)
            {
                ReturnObjectToPool(itemFilter);
                return true;
            }
            return false;
        }

        public bool TryPlaceItemOnGround(Item item, Vector2 location)
        {
            Vector2 itemPosition;
            if (TryFindSpaceForItem(location, out itemPosition))
            {
                var itemGO = GetNewGameObject();
                itemGO.name = item.Name;
                itemGO.tag = "Item";
                itemGO.transform.position = itemPosition;
                itemGO.transform.parent = transform;
                itemGO.GetComponent<ItemFilter>().Item = item;
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool IsItemOnGroundAt(Vector2 location)
        {
            Collider2D collider = CustomRaycast.GetItemColliderAt(location);
            return collider != null && collider.GetComponent<ItemFilter>() != null;
        }

        protected override void OnLevelComplete()
        {
            foreach (Transform child in transform)
            {
                ReturnObjectToPool(child.GetComponent<ItemFilter>());
            }
        }

        ItemFilter GetItemFilter(Vector2 location)
        {
            Collider2D collider = CustomRaycast.GetItemColliderAt(location);
            if (collider != null)
            {
                ItemFilter itemFilter = collider.GetComponent<ItemFilter>();
                Assert.IsNotNull(itemFilter, "Found item without an item filter component.");
                return itemFilter;
            }
            return null;
        }

        bool TryFindSpaceForItem(Vector2 startLocation, out Vector2 foundLocation)
        {
            // Only one item can occupy a spot at a time, so if we try to place an item on top of an existing item,
            // this will search for a nearby open spot instead. 
            int centerY = (int)startLocation.y;
            int centerX = (int)startLocation.x;
            // Search in an expanding ring. Open slots are searched by using raycasts, so we do want to limit
            // the range of our search.
            for (int iteration = 0; iteration < 3; iteration++)
            {
                for (int y = centerY - iteration; y <= centerY + iteration; y++)
                {
                    for (int x = centerX - iteration; x <= centerX + iteration; x++)
                    {
                        Coord coord = new Coord(x, y);
                        if (Map.IsWalkable(coord) && !IsItemOnGroundAt(coord))
                        {
                            foundLocation = coord;
                            return true;
                        }
                    }
                }
            }
            foundLocation = Vector2.zero;
            return false;
        }

        void ReturnObjectToPool(ItemFilter filter)
        {
            numItemsOnGround--;
            filter.ClearItem();
            var go = filter.gameObject;
            go.transform.position = Vector3.zero;
            go.name = "Pooled Item Filter";
            go.SetActive(false);
            pooledGameObjects.Push(go);
        }

        /// <summary>
        /// Retrieves a pooled game object if it can, otherwise will make a new one.
        /// </summary>
        GameObject GetNewGameObject()
        {
            numItemsOnGround++;
            GameObject newGO;
            if (pooledGameObjects.Count > 0)
            {
                newGO = pooledGameObjects.Pop();
                newGO.SetActive(true);
            }
            else
            {
                newGO = Instantiate(itemPrefab);
            }
            return newGO;
        }
    } 
}