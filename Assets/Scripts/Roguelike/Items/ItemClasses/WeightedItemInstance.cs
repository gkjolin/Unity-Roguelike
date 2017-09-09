/* There are several situations where we want to wrap an object with a weight in order to affect a random distribution,
 and it would be very easy to write a generic class to handle all of those situations. Unfortunately Unity does not
 serialize generics (except for lists), so there are a couple of very similar-looking classes that serve only 
 to associate an object with an integer weight.*/

using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using AKSaigyouji.Modules;

namespace AKSaigyouji.Roguelike
{
    /// <summary>
    /// An item template wrapped with a weight.
    /// </summary>
    [Serializable]
    public sealed class WeightedItemTemplate
    {
        public ItemTemplate ItemTemplate { get { return itemTemplate; } }
        [SerializeField] ItemTemplate itemTemplate;

        public int Weight { get { return weight; } }
        [SerializeField] int weight;

        public static ItemTemplate ChooseRandom(IList<WeightedItemTemplate> templates)
        {
            int totalWeight = templates.Sum(item => item.Weight);
            int threshold = UnityEngine.Random.Range(0, totalWeight); 
            int runningWeight = 0;
            foreach (WeightedItemTemplate template in templates)
            {
                runningWeight += template.Weight;
                if (runningWeight > threshold)
                {
                    return template.ItemTemplate;
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