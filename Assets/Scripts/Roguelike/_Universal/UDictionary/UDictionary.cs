using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;

namespace AKSaigyouji.Roguelike
{
    // This non-generic base class enables us to get around an issue with trying to write a property drawer for
    // a generic class. By writing a drawer for the non-generic UDictionary, we can push down the drawer to all
    // implementations of UDictionary<TKey, TValue>, even though we can't write a drawer for UDictionary<TKey, TValue>

    // Note that all the methods which can mutate the state of the dictionary are declared virtual.

    /// <summary>
    /// Exists solely for editor scripting purposes. Using this class in scripts is not recommended.
    /// </summary>
    [Serializable]
    public abstract class UDictionary { }

    /// <summary>
    /// A dictionary that works with Unity's serialization. Note that the types of both the key and value must be
    /// serializable by Unity to work. Also note that it must be subtyped with concrete type values passed in for 
    /// both TKey and TValue, since Unity's serialization does not like generics: this is why it's marked abstract.
    /// </summary>
    [Serializable]
    public abstract class UDictionary<TKey, TValue> : UDictionary, 
        ISerializationCallbackReceiver, IEnumerable<KeyValuePair<TKey, TValue>>
    {
        Dictionary<TKey, TValue> dictionary;

        [SerializeField] TKey[] serializedKeys;
        [SerializeField] TValue[] serializedValues;

        public Dictionary<TKey, TValue>.KeyCollection Keys { get { return dictionary.Keys; } }
        public Dictionary<TKey, TValue>.ValueCollection Values { get { return dictionary.Values; } }

        public int Count { get { return dictionary.Count; } }

        public UDictionary()
        {
            dictionary = new Dictionary<TKey, TValue>();
        }

        public UDictionary(int capacity)
        {
            dictionary = new Dictionary<TKey, TValue>(capacity);
        }

        public UDictionary(IEnumerable<KeyValuePair<TKey, TValue>> entries)
        {
            dictionary = entries.ToDictionary(pair => pair.Key, pair => pair.Value);
        }

        public TValue this[TKey key]
        {
            get { return dictionary[key]; }
            set { dictionary[key] = value; }
        }

        public void Add(TKey key, TValue value)
        {
            dictionary.Add(key, value);
        }
        
        public void Remove(TKey key)
        {
            dictionary.Remove(key);
        }

        public bool ContainsKey(TKey key)
        {
            return dictionary.ContainsKey(key);
        }

        public bool ContainsValue(TValue value)
        {
            return dictionary.ContainsValue(value);
        }

        public bool Contains(KeyValuePair<TKey, TValue> value)
        {
            return dictionary.Contains(value);
        }

        public virtual void Clear()
        {
            dictionary.Clear();
        }

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            return dictionary.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return dictionary.GetEnumerator();
        }

        void ISerializationCallbackReceiver.OnAfterDeserialize()
        {
            if (serializedKeys == null || serializedValues == null) // no saved values
            {
                if (dictionary == null) // ensure the dictionary is always in a valid state after deserialization
                {
                    dictionary = new Dictionary<TKey, TValue>();
                }
                return;
            }

            Assert.IsTrue(serializedKeys.Length == serializedValues.Length);
            dictionary = new Dictionary<TKey, TValue>(serializedKeys.Length);
            for (int i = 0; i < serializedValues.Length; i++)
            {
                dictionary.Add(serializedKeys[i], serializedValues[i]);
            }

            // don't waste run-time memory hanging onto references to the arrays
            serializedKeys = null;
            serializedValues = null;

            OnAfterDeserialize();
        }

        void ISerializationCallbackReceiver.OnBeforeSerialize()
        {
            serializedKeys = dictionary.Keys.ToArray();
            serializedValues = dictionary.Values.ToArray();

            OnBeforeSerialize();
        }

        /// <summary>
        /// Override to add additional serialization logic to a subclass. 
        /// </summary>
        protected virtual void OnAfterDeserialize()
        {

        }

        /// <summary>
        /// Override to add additional serialization logic to a subclass.
        /// </summary>
        protected virtual void OnBeforeSerialize()
        {

        }
    } 
}