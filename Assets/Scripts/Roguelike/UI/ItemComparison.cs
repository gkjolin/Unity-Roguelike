using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace AKSaigyouji.Roguelike
{
    public sealed class ItemComparison : MonoBehaviour
    {
        [SerializeField] ItemDisplayUI ground;
        [SerializeField] ItemDisplayUI inventory;

        void Start()
        {
            Assert.IsNotNull(ground);
            Assert.IsNotNull(inventory);
        }

        public void Display(Item groundItem, Item inventoryItem)
        {
            DisplayItem(ground, groundItem);
            DisplayItem(inventory, inventoryItem);
        }

        public void Display(Item groundItem)
        {
            DisplayItem(ground, groundItem);
            inventory.Disable();
        }

        public void ClearDisplay()
        {
            ground.Disable();
            inventory.Disable();
        }

        void DisplayItem(ItemDisplayUI display, Item item)
        {
            display.Enable();
            display.UpdateDisplay(item);
        }
    } 
}