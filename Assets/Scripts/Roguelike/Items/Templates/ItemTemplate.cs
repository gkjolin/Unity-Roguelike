using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

namespace AKSaigyouji.Roguelike
{
    public abstract class ItemTemplate : ScriptableObject 
    {
        public Sprite Icon { get { return icon; } }
        public abstract InventorySlot Slot { get; }

        [SerializeField] Sprite icon;

        public abstract Item Build(string name);
    } 
}