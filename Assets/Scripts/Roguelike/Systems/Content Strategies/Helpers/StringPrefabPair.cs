/* Unity does not serialize generics, so KeyValuePair<string, GameObject> won't appear in the inspector. This class 
 * exists solely to provide an inspector for this specific case. */

using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

namespace AKSaigyouji.Roguelike
{
    [Serializable]
    /// <summary>
    /// Specialized key value pair for type pair (string, GameObject) for Unity serialization purposes.
    /// </summary>
    public struct StringPrefabPair
    {
        public string Key { get { return key; } }
        public GameObject Value { get { return value; } }

        [SerializeField] string key;
        [SerializeField] GameObject value;

        public StringPrefabPair(string key, GameObject value)
        {
            this.key = key;
            this.value = value;
        }

        // can freely convert implicitly between the corresponding key value pair and this type.

        public static implicit operator KeyValuePair<string, GameObject>(StringPrefabPair pair)
        {
            return new KeyValuePair<string, GameObject>(pair.key, pair.value);
        }

        public static implicit operator StringPrefabPair(KeyValuePair<string, GameObject> pair)
        {
            return new StringPrefabPair(pair.Key, pair.Value);
        }

        public override string ToString()
        {
            return ((KeyValuePair<string, GameObject>)this).ToString();
        }
    } 
}