using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;
using TMPro;
using System.Text;

namespace AKSaigyouji.Roguelike
{
    public sealed class CharacterSheetUI : MonoBehaviour
    {
        [SerializeField] PlayerStats stats;
        [SerializeField] TMP_Text characterSheet;

        const string DISPLAY_FORMAT = "{0}: {1}";

        readonly StringBuilder sb = new StringBuilder();

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
            // this is a temporary hard-coded UI for the character stats - all the strings being built here contribute
            // to a lot of garbage being made every time the character sheet is open. This can be substantially mitigated
            // by generating all the fixed components of the UI once (namely, all the labels) and only updating
            // the numbers themselves. The UI may eventually also be replaced with something hand-drawn. Finally, 
            // it may be a significant optimization to update the character stats in response to something changing
            // the stats. Nonetheless, these changes are a low priority. 
            sb.AppendLine(string.Format("Level: {0}", stats.Level));
            sb.AppendLine(string.Format("Experience: {0}/{1}", stats.Experience, stats.ExperienceToNextLevel));
            sb.AppendLine();
            sb.AppendLine(string.Format("Health: {0}/{1}", stats.CurrentHealth, stats.GetAttribute(Attribute.Health)));
            sb.AppendLine();
            sb.AppendLine(DisplayString(Attribute.Strength));
            sb.AppendLine(DisplayString(Attribute.Dexterity));
            sb.AppendLine(DisplayString(Attribute.Magic));
            sb.AppendLine(DisplayString(Attribute.Vitality));
            sb.AppendLine();
            sb.AppendLine(DisplayString(Attribute.FireResistance));
            sb.AppendLine(DisplayString(Attribute.ColdResistance));
            sb.AppendLine(DisplayString(Attribute.LightningResistance));
            sb.AppendLine(DisplayString(Attribute.PoisonResistance));
            characterSheet.text = sb.ToString();
            sb.Length = 0;
        }

        string DisplayString(Attribute attribute)
        {
            return string.Format(DISPLAY_FORMAT, attribute, stats.GetAttribute(attribute));
        }
    } 
}