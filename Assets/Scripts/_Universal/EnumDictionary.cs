using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;

namespace AKSaigyouji.Roguelike
{
    // We would like to do "where TKey : Enum" but this is prohibited by the C# spec (compiler error). Instead we put
    // the strongest restrictions compatible with Enum: it must be a struct, and must implement the three interfaces 
    // implemented by Enum. Along with the explicit name of the class, this should make it very difficult to 
    // accidentally create an invalid subclass. We also throw a run-time error in the constructor if TKey is not an enum. 

    // An alternative would be to use enumeration classes instead of enums, which would allow for a proper generic constraint
    // but require more work in other areas, such as serialization, enumeration, and inspector displaying, though all
    // of these have reasonable solutions.

    /// <summary>
    /// A dictionary indexed by all the values of an enum. 
    /// </summary>
    /// <typeparam name="TKey">Must be an enum. Should be Unity-serializable.</typeparam>
    /// <typeparam name="TValue">Must have a parameterless constructor. Should be Unity serializable.</typeparam>
    [Serializable]
    public abstract class EnumDictionary<TKey, TValue> : UDictionary<TKey, TValue> 
        where TKey : struct, IComparable, IFormattable, IConvertible
        where TValue : new()
    {
        public EnumDictionary() : base(GenerateDefaultEntries())
        {

        }

        /// <summary>
        /// Generates pairs of the form (TKey, new TValue()) for every distinct TKey.
        /// </summary>
        static IEnumerable<KeyValuePair<TKey, TValue>> GenerateDefaultEntries()
        {
            if (!typeof(TKey).IsEnum)
                throw new Exception("Compile-time error: Invalid class definition. TKey must be an Enum.");

            return Enum.GetValues(typeof(TKey))
                       .OfType<TKey>()
                       .Select(key => new KeyValuePair<TKey, TValue>(key, new TValue()));
        }

        protected override void OnAfterDeserialize()
        {
            // if we add a value to the enum, then any serialized instances of the enum dictionary will have a missing
            // key, so we check for missing enums after every deserialization. This automatically keeps of all our
            // enum dictionaries up to date. Similarly, we remove keys that have been removed from the enum.
            // There's a fair bit of reflection going on here, which is slow, but the speed is acceptable in the context
            // of serialization which requires a lot of reflection anyway.

            bool keysHaveChanged = false;

            foreach (TKey key in Keys.ToArray()) // Make copy so we can remove keys from the dictionary while iterating
            {
                if (!Enum.IsDefined(typeof(TKey), key))
                {
                    Remove(key);
                    keysHaveChanged = true;
                }
            }

            foreach (TKey key in Enum.GetValues(typeof(TKey)))
            {
                if (!ContainsKey(key))
                {
                    Add(key, default(TValue));
                    keysHaveChanged = true;
                }
            }

            if (keysHaveChanged)
            {
                SortDictionaryByValueBackingEnum();
            }
        }

        void SortDictionaryByValueBackingEnum()
        {
            // This is an ugly side effect of not having a compile-time gaurantee that TKey is an Enum. Normally
            // we can cast from an enum to int, since all the underlying types for enums are integer types.
            // We can't cast from TKey to int, but since we know at runtime that TKey will be an enum, we can cast
            // up to a supertype of both TKey and int (in this case, ValueType since TKey is a struct) and then down to int. 
            var pairs = this.OrderBy(pair => (int)(ValueType)pair.Key).ToArray();
            Clear();
            foreach (var pair in pairs)
            {
                Add(pair.Key, pair.Value);
            }
        }
    } 
}