using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace Xi_
{
    public class Test_DataClass : MonoBehaviour
    {
        public List<DataClass> dataClasses = new();

        private void Start()
        {
            dataClasses.Add(new DataClass_1() { info = new Info_1() });
            dataClasses.Add(new DataClass_2() { info = new Info_2() });
        }

        [ContextMenu("Ser")]
        private void Ser()
        {
            foreach (var item in dataClasses)
            {
                item.ToBytes();
            }
        }

        [ContextMenu("Des")]
        private void Des()
        {
            foreach (var item in dataClasses)
            {
                item.FromBytes();
            }
        }

        private void Update()
        {
            var int_1 = (dataClasses.First() as DataClass_1).info.info_1_int;
            print(int_1);
        }
    }

    [CustomEditor(typeof(Test_DataClass))]
    public class Test_DataClassEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            var script = (Test_DataClass)target;

            DrawDefaultInspector();  // 绘制默认Inspector

            if (script.dataClasses != null)
            {
                foreach (var item in script.dataClasses)
                {
                    var type = item.GetType();
                    EditorGUILayout.LabelField("Type:", type.Name);

                    DrawClass(type, item);
                }
            }

            if (GUI.changed)
            {
                EditorUtility.SetDirty(target);
            }
        }

        public static void DrawClass(Type type, object obj, bool space = true)
        {
            // 反射遍历所有字段并绘制
            var fields = type.GetFields(BindingFlags.Public | BindingFlags.Instance);
            foreach (var field in fields)
            {
                object value = field.GetValue(obj);
                var fieldType = field.FieldType;

                if (field.IsDefined(typeof(HideInInspector), false))
                {
                    continue;
                }

                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField(field.Name + ":", GUILayout.Width(150));

                // 根据字段类型决定如何显示和编辑
                if (fieldType == typeof(int))
                {
                    int newValue = EditorGUILayout.IntField((int)value);
                    field.SetValue(obj, newValue);
                }
                else if (fieldType == typeof(float))
                {
                    float newValue = EditorGUILayout.FloatField((float)value);
                    field.SetValue(obj, newValue);
                }
                else if (fieldType == typeof(string))
                {
                    string newValue = EditorGUILayout.TextField((string)value);
                    field.SetValue(obj, newValue);
                }
                else if (fieldType == typeof(bool))
                {
                    bool newValue = EditorGUILayout.Toggle((bool)value);
                    field.SetValue(obj, newValue);
                }
                else if (fieldType.GetInterface(nameof(IInfo)) != null)
                {
                    EditorGUILayout.EndHorizontal();
                    EditorGUI.indentLevel++;
                    DrawClass(fieldType, value, false);
                    EditorGUI.indentLevel--;
                    EditorGUILayout.BeginHorizontal();
                }

                EditorGUILayout.EndHorizontal();
                if (space)
                {
                    EditorGUILayout.Space(10);
                }
            }
        }
    }
}
