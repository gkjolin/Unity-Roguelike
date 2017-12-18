using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;
using TMPro;

namespace AKSaigyouji.Roguelike
{
    public sealed class ItemDisplayUI : MonoBehaviour
    {
        [SerializeField] Image icon;
        [SerializeField] TMP_Text itemName;
        [SerializeField] TMP_Text description;
        [SerializeField] TMP_Text attributes;

        void Start()
        {
            Assert.IsNotNull(icon);
            Assert.IsNotNull(itemName);
            Assert.IsNotNull(description);
            Assert.IsNotNull(attributes);
        }

        public void Disable()
        {
            gameObject.SetActive(false);
        }

        public void Enable()
        {
            gameObject.SetActive(true);
        }

        public void UpdateDisplay(Item item)
        {
            if (item == null)
                throw new ArgumentNullException("item");

            icon.sprite = item.Icon;
            itemName.text = item.Name;
            description.text = item.ItemDescription;
            attributes.text = item.ItemBonuses;
        }
    } 
}