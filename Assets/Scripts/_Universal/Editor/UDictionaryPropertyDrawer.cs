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
    public sealed class UDictionaryPropertyDrawer : PropertyDrawer
    {
        const string KEYS = "serializedKeys";
        const string VALUES = "serializedValues";

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
                var keysProp = property.FindPropertyRelative(KEYS);
                var valuesProp = property.FindPropertyRelative(VALUES);
                for (int i = 0; i < keysProp.arraySize; i++)
                {
                    EditorGUI.PropertyField(left, keysProp.GetArrayElementAtIndex(i), GUIContent.none);
                    EditorGUI.PropertyField(right, valuesProp.GetArrayElementAtIndex(i), GUIContent.none);
                    left.y += EditorGUIUtility.singleLineHeight;
                    right.y += EditorGUIUtility.singleLineHeight;
                }
            }
            EditorGUI.EndProperty();
        }
    } 
}