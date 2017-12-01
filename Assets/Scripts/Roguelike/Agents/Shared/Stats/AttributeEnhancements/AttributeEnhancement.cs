using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;

namespace AKSaigyouji.Roguelike
{
    [Serializable]
    public struct AttributeEnhancement
    {
        public EnhancementPriority Priority { get { return priority; } }
        public Attribute Attribute { get { return attribute; } }
        public int Magnitude { get { return magnitude; } }

        [SerializeField] EnhancementPriority priority;
        [SerializeField] Attribute attribute;
        [SerializeField] int magnitude;

        public AttributeEnhancement(Attribute attribute, EnhancementPriority priority, int magnitude)
        {
            this.attribute = attribute;
            this.priority = priority;
            this.magnitude = magnitude;
        }

        public override string ToString()
        {
            return string.Format("Enhancement of {0} by magnitude {1} via {2}.", attribute, magnitude, priority);
        }
    } 
}