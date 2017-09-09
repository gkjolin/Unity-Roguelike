using System;

namespace AKSaigyouji.Roguelike
{
    public interface IItemFactory
    {
        Item Build(ItemTemplate itemTemplate);
    } 
}