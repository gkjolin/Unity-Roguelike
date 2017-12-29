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
        public EnhancementOperation Operation { get { return operation; } }
        public Attribute Attribute { get { return attribute; } }
        public int Magnitude { get { return magnitude; } }

        [SerializeField] EnhancementOperation operation;
        [SerializeField] Attribute attribute;
        [SerializeField] int magnitude;

        public AttributeEnhancement(Attribute attribute, EnhancementOperation priority, int magnitude)
        {
            this.attribute = attribute;
            this.operation = priority;
            this.magnitude = magnitude;
        }

        public override string ToString()
        {
            return string.Format("Enhancement of {0} by magnitude {1} via {2}.", attribute, magnitude, operation);
        }
    } 
}