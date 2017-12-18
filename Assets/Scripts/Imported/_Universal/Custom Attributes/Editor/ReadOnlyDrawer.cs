/* This can be useful to expose data in the inspector which shouldn't be tampered with, usually for visualization 
 or diagnostic purposes. Unfortunately there does not seem to be a way to get around the limitation of overriding
 any custom property drawers on the type affected by this attribute.*/

using UnityEngine;
using UnityEditor;
using System.Collections;

namespace AKSaigyouji.Roguelike
{
    [CustomPropertyDrawer(typeof(ReadOnlyAttribute))]
    public class ReadOnlyDrawer : PropertyDrawer
    {
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUI.GetPropertyHeight(property);
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);
            GUI.enabled = false;        
            EditorGUI.PropertyField(position, property, label, true);
            GUI.enabled = true;
            EditorGUI.EndProperty();
        }
    }
}