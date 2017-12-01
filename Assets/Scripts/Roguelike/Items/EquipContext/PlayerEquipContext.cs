using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;

namespace AKSaigyouji.Roguelike
{
    public sealed class PlayerEquipContext : IEquipContext
    {
        public IEnumerable<AttributeEnhancement> Enhancements { get { return attributeEnhancements; } }
        readonly List<AttributeEnhancement> attributeEnhancements;

        public PlayerEquipContext()
        {
            attributeEnhancements = new List<AttributeEnhancement>();
        }

        public void ApplyAttributeBuff(AttributeEnhancement buff)
        {
            attributeEnhancements.Add(buff);
        }

        public void Clear()
        {
            attributeEnhancements.Clear();
        }
    } 
}