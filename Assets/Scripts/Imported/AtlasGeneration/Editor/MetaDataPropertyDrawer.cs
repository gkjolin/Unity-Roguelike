using System;
using System.Linq;
using UnityEngine;
using UnityEditor;
using AKSaigyouji.EditorScripting;

namespace AKSaigyouji.AtlasGeneration
{
    [CustomPropertyDrawer(typeof(MetaData))]
    public sealed class MetaDataPropertyDrawer : PropertyDrawer
    {
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            // the number of properties we need to draw under the foldout is equivalent to the number of key value pairs,
            // which is equivalent to the number of colons in the serialized string.
            var metaDataProp = property.FindPropertyRelative("_serializedData");
            int numPairs = metaDataProp.isExpanded  && metaDataProp.stringValue != null ?  1 + metaDataProp.stringValue.Count(c => c == ':') : 0;
            return (1 + numPairs) * EditorHelpers.PROPERTY_HEIGHT_TOTAL;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            label = EditorGUI.BeginProperty(position, label, property);
            Rect propertyPosition = new Rect(position);
            propertyPosition.height = EditorGUIUtility.singleLineHeight;
            var metaDataProp = property.FindPropertyRelative("_serializedData");
            metaDataProp.isExpanded = EditorGUI.Foldout(propertyPosition, metaDataProp.isExpanded, label);
            propertyPosition.y += EditorHelpers.PROPERTY_HEIGHT_TOTAL;
            if (metaDataProp.isExpanded)
            {
                bool changed = false;
                string rawString = metaDataProp.stringValue;
                Rect buttonPosition = new Rect(propertyPosition);
                buttonPosition.x += (EditorGUI.indentLevel + 1) * EditorHelpers.HORIZONTAL_INDENT;
                string buttonText = position.width > 300 ? "Add Key-Value Pair" : "Add";
                buttonPosition.width = position.width > 300 ? 130 : 40;
                if (GUI.Button(buttonPosition, buttonText))
                {
                    rawString += ",key:value";
                    changed = true;
                }
                string[] rawPairs = rawString.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                if (rawPairs.Select(pair => pair.Substring(0, pair.IndexOf(':'))).Distinct().Count() != rawPairs.Length)
                {
                    Rect warningPosition = new Rect(buttonPosition);
                    warningPosition.x += buttonPosition.width + 2;
                    warningPosition.width = 100;
                    EditorGUI.HelpBox(warningPosition, "Duplicate keys.", MessageType.Warning);
                }
                EditorGUI.indentLevel++;
                for (int i = 0; i < rawPairs.Length; i++)
                {
                    propertyPosition.y += EditorHelpers.PROPERTY_HEIGHT_TOTAL;
                    Rect leftPosition = new Rect(propertyPosition);
                    leftPosition.width = propertyPosition.width / 2 - 20;
                    Rect rightPosition = new Rect(leftPosition);
                    rightPosition.x += rightPosition.width + 2;
                    Rect removePosition = new Rect(rightPosition);
                    removePosition.x += rightPosition.width + 10;
                    removePosition.width = 20;
                    string[] pair = rawPairs[i].Split(':');
                    EditorGUI.BeginChangeCheck();
                    string key = EditorGUI.TextField(leftPosition, pair[0]);
                    int indentLevel = EditorGUI.indentLevel;
                    EditorGUI.indentLevel = 0;
                    string value = EditorGUI.TextField(rightPosition, pair[1]);
                    EditorGUI.indentLevel = indentLevel;
                    if (EditorGUI.EndChangeCheck())
                    {
                        rawPairs[i] = string.Format("{0}:{1}", key, value);
                        changed = true;
                    }
                    if (GUI.enabled && GUI.Button(removePosition, "-")) // button won't draw is GUI is disabled
                    {
                        rawPairs[i] = string.Empty;
                        changed = true;
                    }
                }
                EditorGUI.indentLevel--;
                if (changed)
                {
                    string finalString = string.Join(",", rawPairs);
                    metaDataProp.stringValue = finalString;
                }
            }
            EditorGUI.EndProperty();
        }
    } 
}