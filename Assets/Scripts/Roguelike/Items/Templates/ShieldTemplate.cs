using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

namespace AKSaigyouji.Roguelike
{
    [CreateAssetMenu(fileName = "Shield", menuName = "AKSaigyouji/Items/Shield")]
    public sealed class ShieldTemplate : ItemTemplate
    {
        public int Armor { get { return armor; } }

        public override InventorySlot Slot { get { return InventorySlot.Shield; } }

        protected override string ItemDescriptionFormat
        {
            get { return "{0} Armor"; }
        }

        [SerializeField] int armor;

        ArmorEnhancement armorEnhancement = new ArmorEnhancement();

        public string BuildDescription(int armor)
        {
            return string.Format(ItemDescriptionFormat, armor);
        }

        protected override void OnStartBuilding()
        {
            armorEnhancement.Clear();
        }

        protected override bool IsApplicableToItem(AttributeAffix affix)
        {
            return ArmorEnhancement.IsArmorAttribute(affix.Attribute);
        }

        protected override void ApplyToItem(AttributeAffix affix, QualityRoll quality)
        {
            armorEnhancement.ApplyArmorAttribute(affix.Attribute, affix.Priority, affix.Range.Value.Interpolate(quality));
        }

        protected override Item OnFinishBuilding(List<Affix> affixes, string name)
        {
            return new Shield(this, name, affixes, armorEnhancement);
        }
    }
}