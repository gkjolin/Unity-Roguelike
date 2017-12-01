/* The equip context provides the interface needed for affixes to apply their effects when equipped. The context may grow 
 * in complexity over time, as new types of affixes are implemented. This approach was decided upon realizing that there 
 * were many different types of affix effects: the simplest was a stat buff to a particular attribute, e.g. +5 to strength.
 * Then there are skill buffs (+2 to a particular ability), various combat effects (30% chance on being struck to cast a
 * spell), various damage enhancements (+3-9 fire damage, +15% to all cold damage, etc.), spell charges (15 charges of
 * teleport), Oskills (giving a class a skill it doesn't normally have), and other effects as well. */

namespace AKSaigyouji.Roguelike
{
    /// <summary>
    /// Represents an interface for the application of affixes that activate upon being equipped.
    /// </summary>
    public interface IEquipContext
    {
        void ApplyAttributeBuff(AttributeEnhancement buff);
    } 
}