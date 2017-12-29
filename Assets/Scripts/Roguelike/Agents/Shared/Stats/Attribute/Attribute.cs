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
        
            The underlying values are spaced out by cluster and by individual attribute, to make it easy to 
            add new clusters and new attributes inbetween existing ones so as to control ordering while preserving
            the underlying values.
        */

        // primary
        Strength = 1000,
        Dexterity = 1050,
        Magic = 1100,
        Vitality = 1150,
        AllAttributes = 1300,

        // non-physical defense
        FireResistance = 2000,
        ColdResistance = 2050,
        LightningResistance = 2100,
        PoisonResistance = 2150,
        AllResistance = 2300,

        // armor stats
        Armor = 3000,

        // defense against all

        // health related
        Health = 5000,
        HealthPerVitality = 5050,

        // weapon stats
        MinDamage = 9000,
        MaxDamage = 9050,
        WeaponDamage = 9100,
        CritMultiplier = 9150,
        AttackSpeed = 9200,

        // misc
        MoveSpeed = 10000,
        PickupSpeed = 10100,
    } 
}