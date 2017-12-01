using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

namespace AKSaigyouji.Roguelike
{
    public abstract class ItemTemplate : ScriptableObject 
    {
        public string Name { get { return name; } }
        public Sprite Icon { get { return icon; } }
        public abstract InventorySlot Slot { get; }

        [SerializeField] Sprite icon;

        public abstract Item Build(ItemBuildContext context);
    } 
}