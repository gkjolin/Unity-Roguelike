using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;

namespace AKSaigyouji.Roguelike
{
    public sealed class Attack
    {
        public MagnitudeRange PhysicalDamage { get; set; }

        public MagnitudeRange FireDamage { get; set; }
        public MagnitudeRange ColdDamage { get; set; }
        public MagnitudeRange LightningDamage { get; set; }
        public MagnitudeRange PoisonDamage { get; set; }
        public MagnitudeRange MagicDamage { get; set; }

        public int Accuracy { get; set; }
        public int CritChance { get; set; }
        public int CritMultiplier { get; set; }

        public string NameOfAttacker { get; set; }
    } 
}