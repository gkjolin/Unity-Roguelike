using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace AKSaigyouji.Roguelike
{
    public sealed class ItemComparison : GameBehaviour
    {
        [SerializeField] ItemDisplayUI groundDisplaySlot;
        [SerializeField] ItemDisplayUI inventoryDisplaySlot;

        [SerializeField] Ground ground;
        [SerializeField] Inventory inventory;

        void Start()
        {
            Assert.IsNotNull(groundDisplaySlot);
            Assert.IsNotNull(inventoryDisplaySlot);

            Assert.IsNotNull(ground);
            Assert.IsNotNull(inventory);
        }

        protected override void OnPlayerAction()
        {
            UpdateDisplay();
        }

        // No item on ground -> clear display
        // Item on ground, not equippable -> display item on ground
        // item on ground, is equippable -> display both item and matching equipped item for comparison.
        void UpdateDisplay()
        {
            if (ground.IsItemOnGround)
            {
                Item item = ground.ItemOnGround;
                if (item.Slot == InventorySlot.NotEquippable)
                {
                    Display(item);
                }
                else
                {
                    Item matchingItem = inventory.EquippedItems
                                                 .First(equippedItem => equippedItem.Slot == item.Slot);
                    Display(item, matchingItem);
                }
            }
            else
            {
                ClearDisplay();
            }
        }

        public void Display(Item groundItem, Item inventoryItem)
        {
            DisplayItem(groundDisplaySlot, groundItem);
            DisplayItem(inventoryDisplaySlot, inventoryItem);
        }

        public void Display(Item groundItem)
        {
            DisplayItem(groundDisplaySlot, groundItem);
            inventoryDisplaySlot.Disable();
        }

        public void ClearDisplay()
        {
            groundDisplaySlot.Disable();
            inventoryDisplaySlot.Disable();
        }

        void DisplayItem(ItemDisplayUI display, Item item)
        {
            display.Enable();
            display.UpdateDisplay(item);
        }
    } 
}