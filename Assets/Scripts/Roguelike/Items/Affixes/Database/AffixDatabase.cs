using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;

namespace AKSaigyouji.Roguelike
{
    [CreateAssetMenu(fileName = "Affix Database", menuName = "AKSaigyouji/Affixes/Database")]
    public sealed class AffixDatabase : ScriptableObject
    {
        public AffixCollection WeaponAffixes { get { return weaponAffixes; } }
        [SerializeField] AffixCollection weaponAffixes;

        public AffixCollection ArmorAffixes { get { return armorAffixes; } }
        [SerializeField] AffixCollection armorAffixes;
    } 
}