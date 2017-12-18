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
        const int FIELD_OFFSET = 5;
        const int WIDTH_PER_DIGIT = 9;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            Rect rangeRect = EditorGUI.PrefixLabel(position, label);

            var minField = property.FindPropertyRelative("min");
            var maxField = property.FindPropertyRelative("max");

            float minFieldWidth = ComputeIntFieldSize(minField);
            float maxFieldWidth = ComputeIntFieldSize(maxField);

            EditorGUI.indentLevel = 0;
            float x = rangeRect.x;
            float y = rangeRect.y;
            float height = rangeRect.height;
            
            Rect minRect = new Rect(x, y, minFieldWidth, height);
            Rect midRect = new Rect(minRect.xMax + FIELD_OFFSET, y, 2 * WIDTH_PER_DIGIT, height);
            Rect maxRect = new Rect(midRect.xMax + FIELD_OFFSET, y, maxFieldWidth, height);

            maxField.intValue = Mathf.Max(minField.intValue, maxField.intValue);

            EditorGUI.PropertyField(minRect, minField, GUIContent.none);
            EditorGUI.LabelField(midRect, new GUIContent("to"));
            EditorGUI.PropertyField(maxRect, maxField, GUIContent.none);

            EditorGUI.EndProperty();
        }

        static float ComputeIntFieldSize(SerializedProperty prop)
        {
            float textSize = prop.intValue.ToString().Length * WIDTH_PER_DIGIT;
            return Mathf.Max(textSize, 4 * WIDTH_PER_DIGIT); // min size will accommodate 4 digits cleanly
        }
    } 
}