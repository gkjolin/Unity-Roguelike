using System;
using UnityEngine;
using UnityEngine.Assertions;

namespace AKSaigyouji.Roguelike
{
    public sealed class Inventory : GameBehaviour
    {
        public Weapon Weapon { get { return weapon; } }
        public Shield Shield { get { return shield; } }
        public BodyArmor BodyArmor { get { return armor; } }

        [SerializeField] Ground ground;
        [SerializeField] ItemComparison itemComparison;

        [Header("Currently Equipped Items")]
        [SerializeField] Weapon weapon;
        [SerializeField] Shield shield;
        [SerializeField] BodyArmor armor;

        [Header("Starting Items")]
        [SerializeField] WeaponTemplate startingWeapon;
        [SerializeField] ShieldTemplate startingShield;
        [SerializeField] ArmorTemplate startingArmor;

        Item itemOnGround;

        void Start()
        {
            Assert.IsNotNull(ground);
            Assert.IsNotNull(itemComparison);

            weapon = startingWeapon.BuildWeapon("Starter Dagger");
            shield = startingShield.BuildShield("Starter Shield");
            armor = startingArmor.BuildArmor("Starter Armor");
        }

        protected override void OnPlayerAction()
        {
            itemOnGround = ground.GetItemAt(transform.position);
            UpdateItemDisplay();
        }

        public bool TryPickupItem()
        {
            if (itemOnGround != null)
            {
                ground.RemoveItem(transform.position);
                Item replacedItem;
                switch (itemOnGround.Slot)
                {
                    case InventorySlot.Weapon:
                        replacedItem = ReplaceWeapon((Weapon)itemOnGround);
                        break;
                    case InventorySlot.BodyArmor:
                        replacedItem = ReplaceArmor((BodyArmor)itemOnGround);
                        break;
                    case InventorySlot.Shield:
                        replacedItem = ReplaceShield((Shield)itemOnGround);
                        break;
                    case InventorySlot.Consumable:
                        ((IConsumable)itemOnGround).Use(gameObject);
                        return true;
                    default:
                        throw new System.ComponentModel.InvalidEnumArgumentException("Internal error: unidentified inventory slot.");
                }
                ground.TryPlaceItemOnGround(replacedItem, transform.position);
                return true;
            }
            else
            {
                return false;
            }
        }

        void UpdateItemDisplay()
        {
            if (itemOnGround == null)
            {
                itemComparison.ClearDisplay();
            }
            else if (itemOnGround.Slot == InventorySlot.Consumable)
            {
                itemComparison.Display(itemOnGround);
            }
            else
            {
                Item itemToCompare = GetMatchingItem(itemOnGround);
                itemComparison.Display(itemOnGround, itemToCompare);
            }
        }

        /// <summary>
        /// Retrieves the equipped item corresponding to this item's slot.
        /// </summary>
        Item GetMatchingItem(Item item)
        {
            switch (item.Slot)
            {
                case InventorySlot.Weapon:
                    return weapon;
                case InventorySlot.BodyArmor:
                    return armor;
                case InventorySlot.Shield:
                    return shield;
                case InventorySlot.Consumable:
                    throw new ArgumentException("Consumable item does not have matching equipped item.");
                default:
                    throw new System.ComponentModel.InvalidEnumArgumentException("Internal error: unidentified inventory slot.");
            }
        }

        Weapon ReplaceWeapon(Weapon weapon)
        {
            return Replace(weapon, ref this.weapon);
        }

        BodyArmor ReplaceArmor(BodyArmor armor)
        {
            return Replace(armor, ref this.armor);
        }

        Shield ReplaceShield(Shield shield)
        {
            return Replace(shield, ref this.shield);
        }

        T Replace<T>(T newItem, ref T oldItem) where T : Item
        {
            Assert.IsNotNull(oldItem);
            var temp = oldItem;
            oldItem = newItem;
            return temp;
        }
    } 
}