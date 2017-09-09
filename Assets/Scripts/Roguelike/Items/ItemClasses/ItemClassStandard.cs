using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

namespace AKSaigyouji.Roguelike
{
    /// <summary>
    /// An item class that picks from a list with a weighted random probability.
    /// </summary>
    [CreateAssetMenu(fileName = "Standard Item Class", menuName = "AKSaigyouji/Items/Item Class/Standard")]
    public sealed class ItemClassStandard : ItemClass
    {
        public IList<WeightedItemTemplate> ItemTemplates { get { return itemTemplates; } }
        [SerializeField] List<WeightedItemTemplate> itemTemplates;

        public override ItemTemplate FetchItem()
        {
            ItemTemplate itemTemplate = WeightedItemTemplate.ChooseRandom(itemTemplates);
            return itemTemplate;
        }

        void OnValidate()
        {
            if (itemTemplates != null)
            {
                foreach (var item in itemTemplates.Where(template => template != null))
                {
                    item.OnValidate();
                }
            }
        }
    }
}