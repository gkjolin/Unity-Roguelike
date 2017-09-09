using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

namespace AKSaigyouji.Roguelike
{
    [Serializable]
    public abstract class Item
    {
        public abstract Sprite Icon { get; }
        public abstract InventorySlot Slot { get; }
        public abstract string Name { get; } 
        public abstract string DisplayString { get; }

        // We use a separate display string intead of relying on ToString since we want to mandate the implementation
        // of the display string, but overriding ToString is optional.
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

        [SerializeField, ReadOnly] protected T template;
        [SerializeField, ReadOnly] string name;

        public Item(T template, string name)
        {
            this.template = template;
            this.name = name;
        }
    }
}