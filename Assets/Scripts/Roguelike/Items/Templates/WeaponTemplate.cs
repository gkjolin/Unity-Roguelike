using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace AKSaigyouji.Roguelike
{
    [CreateAssetMenu(fileName = "Weapon", menuName = "AKSaigyouji/Items/Weapon")]
    public sealed class WeaponTemplate : ItemTemplate
    {
        public int MinDamage { get { return minDamage; } }
        public int MaxDamage { get { return maxDamage; } }
        public int CritMultiplier { get { return critMultiplier; } }

        public override InventorySlot Slot { get { return InventorySlot.Weapon; } }

        protected override string ItemDescriptionFormat { get { return "{0} - {1} Damage\n{2}x Critical Multiplier"; } }

        [SerializeField] int minDamage;
        [SerializeField] int maxDamage;
        [SerializeField] int critMultiplier;

        WeaponEnhancement tempEnhancement = new WeaponEnhancement();

        public string BuildDescription(int minDamage, int maxDamage, int critMultiplier)
        {
            return string.Format(ItemDescriptionFormat, minDamage, maxDamage, critMultiplier);
        }

        protected override void OnStartBuilding()
        {
            tempEnhancement.Clear();
        }

        protected override bool IsApplicableToItem(AttributeAffix affix)
        {
            return WeaponEnhancement.IsWeaponAttribute(affix.Attribute);
        }

        protected override void ApplyToItem(AttributeAffix affix, QualityRoll quality)
        {
            Assert.IsTrue(IsApplicableToItem(affix));
            MagnitudeRange magnitudeRange = affix.Range.Value;
            int magnitude = magnitudeRange.Interpolate(quality);
            tempEnhancement.ApplyWeaponAttribute(affix.Attribute, affix.Priority, magnitude);
        }

        protected override Item FinishBuilding(List<Affix> affixes, string name)
        {
            return new Weapon(this, name, affixes, tempEnhancement);
        }
    } 
}