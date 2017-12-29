using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;

namespace AKSaigyouji.Roguelike
{
    public sealed class Defense
    {
        public int Armor { get; set; }
        public int CurrentHealth { get; set; }
        public int MaxHealth { get; set; }

        public int FireResistance { get; set; }
        public int ColdResistance { get; set; }
        public int LightningResistance { get; set; }
        public int PoisonResistance { get; set; }

        public int PhysicalEvasion { get; set; }
        public int MagicalEvasion { get; set; }
        
        public int PhysicalDamageSubtraction { get; set; }
        public int NonPhysicalDamageSubstraction { get; set; }

        public int FireAbsorption { get; set; }
        public int ColdAbsorption { get; set; }
        public int LightningAbsorption { get; set; }
        public int PoisonAbsoprtion { get; set; }

        public string NameOfDefender { get; set; }
    } 
}