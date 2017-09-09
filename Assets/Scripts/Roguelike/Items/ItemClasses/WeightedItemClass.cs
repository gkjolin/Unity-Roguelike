using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using AKSaigyouji.Modules;

namespace AKSaigyouji.Roguelike
{
    /// <summary>
    /// An item class wrapped with a weight.
    /// </summary>
    [Serializable]
    public sealed class WeightedItemClass
    {
        public ItemClass ItemClass { get { return itemClass; } }
        [SerializeField] ItemClass itemClass;

        public int Weight { get { return weight; } }
        [SerializeField] int weight;

        public static ItemClass ChooseRandom(IList<WeightedItemClass> itemClasses)
        {
            int totalWeight = itemClasses.Sum(item => item.Weight);
            int threshold = UnityEngine.Random.Range(0, totalWeight);
            int runningWeight = 0;
            foreach (WeightedItemClass weightedClass in itemClasses)
            {
                runningWeight += weightedClass.Weight;
                if (runningWeight > threshold)
                {
                    return weightedClass.ItemClass;
                }
            }
            throw new InvalidOperationException("Internal bug: failed random pick.");
        }

        public void OnValidate()
        {
            weight = Mathf.Max(1, weight);
        }
    } 
}