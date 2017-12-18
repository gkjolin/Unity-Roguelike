namespace AKSaigyouji.Roguelike
{
    public enum Attribute : int
    {
        /*
            The idea behind the specified values is to allow for the addition of new attributes in the future without
            breaking existing serialized attributes. e.g. if we had two consecutive attributes Vitality and FireResistance,
            and we added Energy after Vitality, then any existing asset referencing FireResistance would reference Energy
            instead. This could be avoided by only adding new attributes at the end, but then we end up with a confusing
            ordering, which would appear in dropdowns and the natural sort order defined by enums.
            Specifying values allows us to group related attributes into clusters. In the future, the clusters could
            also be used to create a custom dropdown hierarchy using a custom property drawer.
        
            The pattern is to assign each cluster of attributes a multiple of 1000, and then add a multiple of 50 for each
            attribute within that cluster. If we need to add a new attribute in the future, we can take the halfway value 
            between two existing values. This gives space for up to 50 attributes between any two original attributes,
            which is way more than we need.
        */

        // primary
        Strength = 1000,
        Dexterity = 1050,
        Magic = 1100,
        Vitality = 1150,

        // non-physical defense
        FireResistance = 2000,
        ColdResistance = 2050,
        LightningResistance = 2100,
        PoisonResistance = 2150,

        // physical defense
        Armor = 3000,

        // defense against all

        // health related
        Health = 5000,
        HealthPerVitality = 5050,

        // offensive, physical
        MinDamage = 9000,
        MaxDamage = 9050,
        CritMultiplier = 9150,

        // misc
        MoveSpeed = 10000,
        AttackSpeed = 10050,
        PickupSpeed = 10100,
    } 
}