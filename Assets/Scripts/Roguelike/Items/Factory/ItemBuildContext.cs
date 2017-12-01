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
        // This will hold things such as player level, item level, environmental tags,
        // magic find, temporary bonuses, and anything else relevant to item creation. 
        // These properties will be added as they become available. 

        /// <summary>
        /// Only available if the number of affixes is greater than 0.
        /// </summary>
        public AffixDatabase AffixDatabase
        {
            get
            {
                if (numAffixes == 0) throw new InvalidOperationException("Cannot access affixes when NumAffixes is 0");
                return affixDatabase;
            }
        }
        readonly AffixDatabase affixDatabase;

        public int NumAffixes { get { return numAffixes; } }
        readonly int numAffixes;

        /// <summary>
        /// Used for a mundane item with no modifications to the original template. Essentially a (valid) null object.
        /// </summary>
        public static readonly ItemBuildContext MundaneContext = new ItemBuildContext();

        private ItemBuildContext()
        {
            numAffixes = 0;
        }

        public ItemBuildContext(AffixDatabase affixDatabase)
        {
            this.affixDatabase = affixDatabase;
            numAffixes = 2; // temporary hard-coded value for testing purposes
        }
    } 
}