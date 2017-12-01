using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.Assertions;

namespace AKSaigyouji.Roguelike
{
    [CustomPropertyDrawer(typeof(MagnitudeRange))]
    public sealed class MagnitudeRangeDrawer : PropertyDrawer
    {
        const int FIELD_WIDTH = 60;
        const int FIELD_OFFSET = 5;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            Rect rangeRect = EditorGUI.PrefixLabel(position, label);
           
            Rect minRect = new Rect(rangeRect.x, rangeRect.y, FIELD_WIDTH, rangeRect.height);
            Rect maxRect = new Rect(rangeRect.x + FIELD_WIDTH + FIELD_OFFSET, rangeRect.y, FIELD_WIDTH, rangeRect.height);

            var minField = property.FindPropertyRelative("min");
            var maxField = property.FindPropertyRelative("max");

            maxField.intValue = Mathf.Max(minField.intValue, maxField.intValue);

            EditorGUI.PropertyField(minRect, minField, GUIContent.none);
            EditorGUI.PropertyField(maxRect, maxField, GUIContent.none);

            EditorGUI.EndProperty();
        }
    } 
}