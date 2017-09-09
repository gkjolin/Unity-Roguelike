using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

namespace AKSaigyouji.Roguelike
{
    /// <summary>
    /// An item class that will pick an item class to pick from, rather than picking an item directly.
    /// </summary>
    [CreateAssetMenu(fileName = "Recursive Item Class", menuName = "AKSaigyouji/Items/Item Class/Recursive")]
    public sealed class ItemClassRecursive : ItemClass
    {
        public IList<WeightedItemClass> ItemClasses { get { return itemClasses; } }
        [SerializeField] List<WeightedItemClass> itemClasses;

        public override ItemTemplate FetchItem()
        {
            ItemClass itemClass = WeightedItemClass.ChooseRandom(itemClasses);
            if (itemClass == null)
            {
                return null;
            }
            else
            {
                return itemClass.FetchItem();
            }
        }

        void OnValidate()
        {
            if (itemClasses != null)
            {
                foreach (var item in itemClasses.Where(cl => cl != null))
                {
                    item.OnValidate();
                }
            }
        }
    }
}