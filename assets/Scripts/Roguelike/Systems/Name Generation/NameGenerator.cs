/* Diablo 2 uses a simple name generator: {prefix} {suffix} the {appelation}, where prefix, suffix and
 * appelation are randomly chosen from lists without concern for the type of monster.*/

using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

namespace AKSaigyouji.Roguelike
{
    [CreateAssetMenu(fileName = "Name Generator", menuName = "AKSaigyouji/Name Generator")]
    public sealed class NameGenerator : ScriptableObject
    {
        public IEnumerable<string> Prefixes { get { return prefixes; } }
        public IEnumerable<string> Suffixes { get { return suffixes; } }
        public IEnumerable<string> Appelations { get { return appelations; } }

        [SerializeField] string[] prefixes;
        [SerializeField] string[] suffixes;
        [SerializeField] string[] appelations;

        public string BuildName()
        {
            return string.Format("{0} {1} the {2}", PickRandom(prefixes), PickRandom(suffixes), PickRandom(appelations));
        }

        public string PickRandom(string[] arr)
        {
            return arr[UnityEngine.Random.Range(0, arr.Length)];
        }
    } 
}