using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

namespace AKSaigyouji.Roguelike
{
    public sealed class ItemFactory : MonoBehaviour
    {
        [SerializeField] AffixDatabase affixDatabase;

        public Item Build(ItemTemplate template)
        {
            return template.Build(new ItemBuildContext(affixDatabase));
        }
    } 
}