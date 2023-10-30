using System;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Xi.Extent.Attribute
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public class ReadOnlyAttribute : PropertyAttribute
    {
    }

#if UNITY_EDITOR
    [CustomPropertyDrawer(typeof(ReadOnlyAttribute))]
    public class ReadOnlyAttributeDrawer : PropertyDrawer
    {
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label) => EditorGUI.GetPropertyHeight(property, label, true);
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            bool guiEnabled = GUI.enabled;
            GUI.enabled = false;
            EditorGUI.BeginProperty(position, label, property); // 这一句要加上，否则会导致属性的序列化、更新、撤销、验证和约束等方面出现问题
            EditorGUI.PropertyField(position, property, label, true);
            EditorGUI.EndProperty();    // 搭配这一句使用
            GUI.enabled = guiEnabled;
        }
    }
#endif
}