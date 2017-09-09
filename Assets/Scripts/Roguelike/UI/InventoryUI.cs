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
        [SerializeField] PlayerStats stats;
        [SerializeField] Inventory inventory;
        [SerializeField] Text characterSheet;

        [SerializeField] ItemDisplayUI weapon;
        [SerializeField] ItemDisplayUI armor;
        [SerializeField] ItemDisplayUI shield;

        void Start()
        {
            Assert.IsNotNull(characterSheet);
            Assert.IsNotNull(stats);
            Assert.IsNotNull(inventory);

            Assert.IsNotNull(weapon);
            Assert.IsNotNull(armor);
            Assert.IsNotNull(shield);
        }

        public void ToggleInventory()
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
            UpdateCharacterStats();
            UpdateItems();
        }

        void HideInventory()
        {
            gameObject.SetActive(false);
        }

        void UpdateCharacterStats()
        {
            characterSheet.text = stats.GetFormattedStatus();
        }

        void UpdateItems()
        {
            weapon.UpdateDisplay(inventory.Weapon);
            armor.UpdateDisplay(inventory.BodyArmor);
            shield.UpdateDisplay(inventory.Shield);
        }
    } 
}