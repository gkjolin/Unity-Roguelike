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
        public bool IsItemOnGround { get { return itemOnGround != null; } }
        public Item ItemOnGround { get { return itemOnGround.Item; } }

        [SerializeField] Transform player;
        [SerializeField] GameObject itemPrefab;
        [SerializeField] MapFilter mapFilter;

        IMap Map { get { return mapFilter.Map; } }
        Stack<GameObject> pooledGameObjects = new Stack<GameObject>();

        // useful diagnostic info
        [SerializeField, ReadOnly] int numItemsOnGround = 0;

        ItemFilter itemOnGround;

        void Start()
        {
            Assert.IsNotNull(player);
            Assert.IsNotNull(itemPrefab);
            Assert.IsNotNull(mapFilter);
        }

        protected override void OnPlayerAction()
        {
            ScanGround();
        }

        protected override void OnLevelComplete()
        {
            foreach (Transform child in transform)
            {
                ReturnObjectToPool(child.GetComponent<ItemFilter>());
            }
        }

        /// <summary>
        /// Removes the item from the ground at this location and returns it.
        /// </summary>
        public Item PickUpItem()
        {
            if (itemOnGround == null)
                throw new InvalidOperationException("No item on ground to pick up.");

            Item item = itemOnGround.Item;
            ReturnObjectToPool(itemOnGround);
            return item;
        }

        /// <summary>
        /// Drops the item on the ground at the given location. If an item is already on the ground at that
        /// location, will attempt to find adjacent location to drop the item. If no location is found, the item
        /// will simply be destroyed.
        /// </summary>
        /// <returns>Boolean indicating whether a location was successfully found for the item.</returns>
        public bool DropItem(Item item, Vector2 dropLocation)
        {
            Vector2 itemPosition;
            if (TryFindSpaceForItem(dropLocation, out itemPosition))
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

        bool IsItemOnGroundAt(Vector2 location)
        {
            Collider2D collider = CustomRaycast.GetItemColliderAt(location);
            return collider != null && collider.GetComponent<ItemFilter>() != null;
        }

        void ScanGround()
        {
            itemOnGround = GetItemFilter(player.position);
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
            filter.Clear();
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