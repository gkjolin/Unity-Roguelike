using System;

namespace AKSaigyouji.Roguelike
{
    [Serializable]
    public class BodyArmor : Item<ArmorTemplate>
    {
        public int Armor { get { return template.Armor; } }

        public override string DisplayString { get { return displayString; } }

        readonly string displayString;

        public BodyArmor(ArmorTemplate template, string name) : base(template, name)
        {
            displayString = string.Format("{0} armor", Armor);
        }
    } 
}