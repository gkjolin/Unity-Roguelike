/* At the moment this class handles two responsibilities: one is to expose editor functionality necessary to 
 manage all the affixes that will be used in the game. The other is to provide efficient run-time access to affixes
 when building items. In the future, this will likely be split into a second class, especially
 since once the application starts, all the affixes are locked-in and not subject to change.*/

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;

namespace AKSaigyouji.Roguelike
{
    [CreateAssetMenu(fileName = "Affix Database", menuName = "AKSaigyouji/Affixes/Database")]
    public sealed class AffixDatabase : ScriptableObject
    {
        public AffixCollection GeneralAffixes { get { return generalAffixes; } }
        [SerializeField] AffixCollection generalAffixes;

        public AffixCollection ArmorAffixes { get { return armorAffixes; } }
        [SerializeField] AffixCollection armorAffixes;

        public AffixCollection WeaponAffixes { get { return weaponAffixes; } }
        [SerializeField] AffixCollection weaponAffixes;

        // reusable lists for holding collections of affixes
        List<AffixDefinition> tempDefinitionsA = new List<AffixDefinition>();
        List<AffixDefinition> tempDefinitionsB = new List<AffixDefinition>();

        void Awake()
        {
            Assert.IsNotNull(generalAffixes);
            Assert.IsNotNull(weaponAffixes);
            Assert.IsNotNull(armorAffixes);

            Assert.IsTrue(generalAffixes.Affixes.All(aff => aff != null));
            Assert.IsTrue(armorAffixes.Affixes.All(aff => aff != null));
            Assert.IsTrue(weaponAffixes.Affixes.All(aff => aff != null));
        }

        public List<AffixDefinition> PickRandomAffixes(AffixCollection collection, int numPrefixes, int numSuffixes)
        {
            if (numPrefixes < 0) throw new ArgumentOutOfRangeException("Cannot have a negative number of prefixes.");
            if (numSuffixes < 0) throw new ArgumentOutOfRangeException("Cannot have a negative number of suffixes.");
            if (numPrefixes + numSuffixes > 50) throw new ArgumentOutOfRangeException("Cannot have 50+ affixes.");

            var prefixes = tempDefinitionsA;
            var suffixes = tempDefinitionsB;

            prefixes.Clear();
            suffixes.Clear();

            foreach (AffixDefinition affix in collection.Affixes)
            {
                var affixList = affix.IsPrefix ? prefixes : suffixes;
                affixList.Add(affix);
            }

            var chosenAffixes = new List<AffixDefinition>(numPrefixes + numSuffixes);

            SelectRandomAffixes(prefixes, Mathf.Min(prefixes.Count, numPrefixes), chosenAffixes);
            SelectRandomAffixes(suffixes, Mathf.Min(suffixes.Count, numSuffixes), chosenAffixes);

            prefixes.Clear();
            suffixes.Clear();

            return chosenAffixes;
        }

        /// <summary>
        /// Selects random affixes without repetition from a list. 
        /// Note that the affixes will be swapped around in the list.
        /// </summary>
        /// <param name="affixes">The list of affixes to randomly select from.</param>
        /// <param name="numChoices">Number of affixes to choose. Can't be larger than the number of affixes.</param>
        /// <param name="chosen">Chosen affixes are added to this list.</param>
        void SelectRandomAffixes(List<AffixDefinition> affixes, int numChoices, List<AffixDefinition> chosen)
        {
            // This is an efficient way to pick n random elements from a list without allocating unnecessary memory
            // Swap the randomly chosen pick into the final valid slot, then repeat on the list minus that last slot
            for (int i = affixes.Count - 1; i >= affixes.Count - numChoices; i--)
            {
                int chosenIndex = UnityEngine.Random.Range(0, i + 1);
                chosen.Add(affixes[chosenIndex]);
                Swap(affixes, chosenIndex, i);
            }
        }

        static void Swap(List<AffixDefinition> affixes, int a, int b)
        {
            var temp = affixes[a];
            affixes[a] = affixes[b];
            affixes[b] = temp;
        }
    } 
}