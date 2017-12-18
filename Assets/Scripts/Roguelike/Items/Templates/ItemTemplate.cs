using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace AKSaigyouji.Roguelike
{
    public abstract class ItemTemplate : ScriptableObject 
    {
        public string Name { get { return name; } }
        public Sprite Icon { get { return icon; } }

        public abstract InventorySlot Slot { get; }
        protected abstract string ItemDescriptionFormat { get; }

        [SerializeField] Sprite icon;

        bool isBuilding;
        protected List<Affix> tempAffixes;

        void OnEnable()
        {
            // this is mainly to handle the in-editor situation where an exception is thrown while building an item,
            // causing the temp data to be in an invalid state when starting the application.
            isBuilding = false;
            tempAffixes = null;
        }

        /// <summary>
        /// Build an instance of this item directly from template, with no special bonuses.
        /// </summary>
        public Item BuildMundane(string name)
        {
            StartBuilding();
            return FinishBuilding(name);
        }

        public void StartBuilding()
        {
            if (isBuilding)
                throw new InvalidOperationException("Already building.");

            isBuilding = true;
            tempAffixes = new List<Affix>(6);
            OnStartBuilding();
        }

        /// <summary>
        /// Override to add initialization steps when item building has begun. Will be called after StartBuilding.
        /// </summary>
        protected virtual void OnStartBuilding() { }

        /// <summary>
        /// Apply an attribute affix to the item. The item will consume the affix directly if it can (e.g. weapons may
        /// consume MinDamage directly by adding it to their minimum damage), in which case the affix will not be applied
        /// to the player.
        /// </summary>
        public void AddAffix(AffixDefinition affix, QualityRoll quality)
        {
            if (!isBuilding)
                throw new InvalidOperationException("Must start building before adding affixes.");

            if (affix == null)
                throw new ArgumentNullException("affix");

            // Double-dispatch could be used to avoid the type-sniffing, but this is so much simpler,
            // and we're unlikely to need to extend this
            AttributeAffix attributeAffix = affix as AttributeAffix;
            bool applyToItem = (attributeAffix != null) &&  IsApplicableToItem(attributeAffix);
            if (applyToItem)
            {
                ApplyToItem(attributeAffix, quality);
            }
            tempAffixes.Add(new Affix(affix, quality, applyToItem));
        }

        /// <summary>
        /// Finish building, returning the completed item.
        /// </summary>
        /// <returns>The complete item with all added bonuses.</returns>
        public Item FinishBuilding(string name)
        {
            if (!isBuilding)
                throw new InvalidOperationException("Must start building before finishing.");

            if (string.IsNullOrEmpty(name))
                throw new ArgumentException("Must have a valid non-empty name.");

            isBuilding = false;
            return FinishBuilding(tempAffixes, name);
        }

        /// <summary>
        /// Can the affix be applied directly to the item?
        /// </summary>
        protected virtual bool IsApplicableToItem(AttributeAffix affix)
        {
            return false;
        }

        /// <summary>
        /// Apply the affix, identified by IsApplicableToItem, to the item. 
        /// </summary>
        protected virtual void ApplyToItem(AttributeAffix affix, QualityRoll quality)
        {
            throw new ArgumentException("Affix not applicable to item.");
        }

        protected abstract Item FinishBuilding(List<Affix> affixes, string name);
    } 
}