using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;

namespace AKSaigyouji.Roguelike
{
    public abstract class AffixDefinition : ScriptableObject
    {
        public string Name { get { return name; } }
        public AffixLocation Location { get { return location; } }

        [SerializeField] AffixLocation location;

        public abstract void OnEquip(IEquipContext context, float quality);
    } 
}