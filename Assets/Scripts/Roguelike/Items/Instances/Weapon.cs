using System;

namespace AKSaigyouji.Roguelike
{
    [Serializable]
    public class Weapon : Item<WeaponTemplate>
    {
        public int MinDamage { get { return template.MinDamage; } }
        public int MaxDamage { get { return template.MaxDamage; } }
        public int CritMultiplier { get { return template.CritMultiplier; } }

        public override string DisplayString { get { return displayString; } }

        readonly string displayString;

        const string displayStringFormat = "{0}-{1} damage"
            + "\n" + "x{2} crit multiplier";

        public Weapon(WeaponTemplate template, string name) : base(template, name)
        {
            displayString = string.Format(displayStringFormat, MinDamage, MaxDamage, CritMultiplier);
        }
    } 
}