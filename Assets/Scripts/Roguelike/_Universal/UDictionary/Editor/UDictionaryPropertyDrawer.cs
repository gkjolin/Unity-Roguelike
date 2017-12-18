using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEditor;

namespace AKSaigyouji.Roguelike
{
    [CustomPropertyDrawer(typeof(UDictionary), true)]
    public class UDictionaryPropertyDrawer : PropertyDrawer
    {
        const string KEYS = "serializedKeys";
        const string VALUES = "serializedValues";
        const string FIX_KEYS = "fixKeys";

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            float height = EditorGUIUtility.singleLineHeight;
            if (property.isExpanded)
            {
                height += property.FindPropertyRelative(KEYS).arraySize * EditorGUIUtility.singleLineHeight;
            }
            return height;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);
            position.height = EditorGUIUtility.singleLineHeight;
            if (property.isExpanded = EditorGUI.Foldout(position, property.isExpanded, label))
            {
                Rect left = position;
                left.width /= 2;
                left.y += EditorGUIUtility.singleLineHeight;
                Rect right = left;
                right.x += left.width;
                EditorGUI.indentLevel++;
                var keys = property.FindPropertyRelative(KEYS);
                var values = property.FindPropertyRelative(VALUES);

                for (int i = 0; i < keys.arraySize; i++)
                {
                    EditorGUI.PropertyField(left, keys.GetArrayElementAtIndex(i), GUIContent.none);
                    EditorGUI.PropertyField(right, values.GetArrayElementAtIndex(i), GUIContent.none);
                    left.y += EditorGUIUtility.singleLineHeight;
                    right.y += EditorGUIUtility.singleLineHeight;
                }
            }
            EditorGUI.EndProperty();
        }
    } 
}