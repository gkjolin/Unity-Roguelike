using System;
using System.Collections;
using System.Collections.Generic;

namespace AKSaigyouji.Roguelike
{
    [Serializable]
    public class Shield : Item<ShieldTemplate>
    {
        public int Armor { get { return template.Armor; } }

        public override string DisplayString { get { return displayString; } }

        readonly string displayString;

        public Shield(ShieldTemplate template, string name, IEnumerable<Affix> affixes) : base(template, name, affixes)
        {
            displayString = string.Format("{0} armor", Armor);
        }

        public override Item Equip(IInventory inventory)
        {
            Item old = inventory.Shield;
            inventory.Shield = this;
            return old;
        }
    } 
}