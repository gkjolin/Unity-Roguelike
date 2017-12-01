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
            return EditorGUI.GetPropertyHeight(property) + 200;
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