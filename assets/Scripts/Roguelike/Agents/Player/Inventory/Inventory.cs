using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Assertions;

namespace AKSaigyouji.Roguelike
{
    public sealed class Inventory : GameBehaviour, IInventory
    {
        // This is not an ideal way to manage the equipped items, but it's a simple one. Will investigate alternatives
        // when it's more clear what other systems need to interact with the equipped items and how.
        public Weapon Weapon { get { return weapon; } }
        public Shield Shield { get { return shield; } }
        public BodyArmor BodyArmor { get { return armor; } }

        public IEnumerable<Item> EquippedItems
        {
            get
            {
                yield return Weapon;
                yield return Shield;
                yield return BodyArmor;
            }
        }

        // Only through the IInventory interface should the items be changed.
        Weapon IInventory.Weapon { get { return Weapon; } set { weapon = value; } }
        Shield IInventory.Shield { get { return Shield; } set { shield = value; } }
        BodyArmor IInventory.BodyArmor { get { return BodyArmor; } set { armor = value; } }

        [SerializeField] Ground ground;
        [SerializeField] GameObject playerGO;
        [SerializeField] ItemFactory itemFactory;

        [Header("Currently Equipped Items")]
        [SerializeField, ReadOnly] Weapon weapon;
        [SerializeField, ReadOnly] Shield shield;
        [SerializeField, ReadOnly] BodyArmor armor;

        [Header("Starting Items")]
        [SerializeField] WeaponTemplate startingWeapon;
        [SerializeField] ShieldTemplate startingShield;
        [SerializeField] ArmorTemplate startingArmor;

        [SerializeField] InventoryChangeEvent inventoryChanged;

        void Start()
        {
            Assert.IsNotNull(ground);

            EquipStartingItem(startingWeapon);
            EquipStartingItem(startingShield);
            EquipStartingItem(startingArmor);
            inventoryChanged.Invoke(EquippedItems);
        }

        public bool TryPickupItem()
        {
            if (!ground.IsItemOnGround)
                return false;

            Item item = ground.PickUpItem();
            if (item.CanEquip)
            {
                Item oldItem = item.Equip(this);
                ground.DropItem(oldItem, transform.position);
                inventoryChanged.Invoke(EquippedItems);
            }
            else
            {
                // might be better to fold this into Equip so that all items can be 'equipped'
                IConsumable itemAsConsumable = (IConsumable)item;
                itemAsConsumable.Use(transform.parent.gameObject);
            }
            return true;
        }

        void EquipStartingItem(ItemTemplate startingItem)
        {
            Assert.IsNotNull(startingItem);
            startingItem.BuildMundane(startingItem.Name)
                        .Equip(this);
        }
    } 
}