using System;
using System.Linq;
using System.Collections.Generic;
using AKSaigyouji.Modules;

namespace AKSaigyouji.Roguelike
{
    public abstract class ItemClass : Module
    {
        public abstract ItemTemplate FetchItem();
    } 
}