using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;

namespace AKSaigyouji.Roguelike
{
    /// <summary>
    /// Holds all the contextual information needed to build an item.
    /// </summary>
    public sealed class ItemBuildContext
    {
        public QualityRoll Quality { get { return new QualityRoll(UnityEngine.Random.Range(0f, 1f)); } }

        public ItemFactory Factory { get { return factory; } }
        readonly ItemFactory factory;

        public ItemBuildContext(ItemFactory factory)
        {
            this.factory = factory;
        }
    } 
}