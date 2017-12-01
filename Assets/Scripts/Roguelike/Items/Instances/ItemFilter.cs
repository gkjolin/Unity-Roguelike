using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

namespace AKSaigyouji.Roguelike
{
    /// <summary>
    /// Used to represent items on the map, i.e. when not in the inventory.
    /// </summary>
    public sealed class ItemFilter : MonoBehaviour
    {
        public Item Item
        {
            get { return item; }
            set
            {
                item = value;
                gameObject.GetComponent<SpriteRenderer>().sprite = item.Icon;
            }
        }
        [SerializeField] Item item;

        public void Clear()
        {
            item = null;
            GetComponent<SpriteRenderer>().sprite = null;
        }
    } 
}