/* This enum was defined to provide consistency and control over how attributes are calculated. If we have a 
 * multiplicative buff, e.g. +100% damage, then whether we apply an additive buff, e.g. +20 damage, before or after
 * the multiplicative buff makes a large difference. Having multiple tiers allows certain enhancements to be a lot
 * more effective than others: additive passes are more effective when applied earlier, while multiplicative enhancements
 * are more effective when applied later. Override passes are meant for very special effects. e.g. an item may 
 * set crit chance to 100% but eliminate all forms of elemental damage as a tradeoff. Using the override tiers makes it 
 * easy to apply such effects without having to apply conditional logic to all buffs to check for the presence of 
 * overrides. */

namespace AKSaigyouji.Roguelike
{
    /// <summary>
    /// Indicates the priority in which an enhancement should be applied. 
    /// </summary>
    public enum EnhancementOperation
    {
        Additive = 1,
        Multiplicative = 2,
        Override = 250,
    } 
}