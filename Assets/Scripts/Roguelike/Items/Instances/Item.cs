using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace AKSaigyouji.Roguelike
{
    [Serializable]
    public abstract class Item
    {
        public abstract Sprite Icon { get; }
        public abstract InventorySlot Slot { get; }
        public abstract string Name { get; } 
        public abstract string DisplayString { get; }
        
        // The divison of item into equippable (weapon, armor), and non-equippable items (potion, quest item)
        // is not ideal. It might be better to splinter the latter out of the item hierarchy, and perhaps to use
        // interfaces to unite their common behaviour (such as their ability to drop from enemies). 
        public bool CanEquip { get { return Slot != InventorySlot.NotEquippable; } }

        // Equip depends on the item's type so it's a method on Item, not Inventory.

        /// <summary>
        /// Equip this item. Returns the previously equipped item in this slot, if applicable.
        /// </summary>
        public abstract Item Equip(IInventory inventory);

        /// <summary>
        /// Applies the item's effects (i.e. affixes and other special affects) to the player.
        /// </summary>
        public abstract void ApplyEffects(IEquipContext context);

        public override string ToString()
        {
            return string.Format("{0}, in {1} slot.", Name, Slot);
        }
    }
    
    [Serializable]
    public abstract class Item<T> : Item where T : ItemTemplate
    {
        public override InventorySlot Slot { get { return template.Slot; } }
        public override Sprite Icon { get { return template.Icon; } }
        public override string Name { get { return name; } }

        [SerializeField] List<Affix> affixes;
        [SerializeField] string name;
        [SerializeField] protected T template;

        public Item(T template, string name, IEnumerable<Affix> affixes)
        {
            Assert.IsNotNull(template);
            Assert.IsNotNull(name);
            Assert.IsNotNull(affixes);

            this.template = template;
            this.name = name;
            this.affixes = affixes.Any() ? affixes.ToList() : null;
        }

        public override sealed void ApplyEffects(IEquipContext context)
        {
            if (affixes != null)
            {
                foreach (Affix affix in affixes)
                {
                    affix.Equip(context);
                }
            }
        }
    }
}