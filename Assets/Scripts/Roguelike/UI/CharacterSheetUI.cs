using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace AKSaigyouji.Roguelike
{
    public sealed class CharacterSheetUI : MonoBehaviour
    {
        [SerializeField] PlayerStats stats;
        [SerializeField] Text characterSheet;

        void Start()
        {
            Assert.IsNotNull(characterSheet);
            Assert.IsNotNull(stats);
        }

        public void Toggle()
        {
            if (gameObject.activeInHierarchy)
            {
                gameObject.SetActive(false);
            }
            else
            {
                gameObject.SetActive(true);
                UpdateCharacterStats();
            }
        }

        void UpdateCharacterStats()
        {
            characterSheet.text = stats.GetFormattedStatus();
        }
    } 
}