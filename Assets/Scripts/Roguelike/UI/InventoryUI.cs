using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace AKSaigyouji.Roguelike
{
    public sealed class InventoryUI : MonoBehaviour
    {
        [SerializeField] Inventory inventory;

        [SerializeField] ItemDisplayUI weapon;
        [SerializeField] ItemDisplayUI armor;
        [SerializeField] ItemDisplayUI shield;

        void Awake()
        {
            Assert.IsNotNull(inventory);

            Assert.IsNotNull(weapon);
            Assert.IsNotNull(armor);
            Assert.IsNotNull(shield);
        }

        public void Toggle()
        {
            if (gameObject.activeInHierarchy)
            {
                HideInventory();
            }
            else
            {
                DisplayInventory();
            }
        }

        void DisplayInventory()
        {
            gameObject.SetActive(true);
            UpdateItems();
        }

        void HideInventory()
        {
            gameObject.SetActive(false);
        }

        void UpdateItems()
        {
            weapon.UpdateDisplay(inventory.Weapon);
            armor.UpdateDisplay(inventory.BodyArmor);
            shield.UpdateDisplay(inventory.Shield);
        }
    } 
}