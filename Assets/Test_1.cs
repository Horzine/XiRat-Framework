using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace Xi_
{
    [Serializable]
    public class TestData
    {
        public int a;
    }

    [Serializable]
    public class Test_Data_1 : TestData
    {
        public int b;
    }

    public class Test_1 : MonoBehaviour
    {
        public TestData testData;
        public Test_Data_1 testData_2;
        public List<TestData> testDatas;

        private void Awake() => testData = new Test_Data_1();

        [ContextMenu("Modify")]
        public void Modify()
        {
            string path = PrefabUtility.GetPrefabAssetPathOfNearestInstanceRoot(this);
            testData = new Test_Data_1() { a = 10, b = 20 };
            PrefabUtility.ApplyObjectOverride(this, path, InteractionMode.UserAction);
        }
    }

    [CustomEditor(typeof(Test_1))]
    public class MyScriptEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            var script = (Test_1)target;

            DrawDefaultInspector();  // 绘制默认Inspector

            // 检查变量并动态绘制
            if (script.testData != null)
            {
                var type = script.testData.GetType();
                EditorGUILayout.LabelField("Type:", type.Name);

                // 反射遍历所有字段并绘制
                var fields = type.GetFields(BindingFlags.Public | BindingFlags.Instance);
                foreach (var field in fields)
                {
                    object value = field.GetValue(script.testData);
                    var fieldType = field.FieldType;

                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField(field.Name + ":", GUILayout.Width(150));

                    // 根据字段类型决定如何显示和编辑
                    if (fieldType == typeof(int))
                    {
                        int newValue = EditorGUILayout.IntField((int)value);
                        field.SetValue(script.testData, newValue);
                    }
                    else if (fieldType == typeof(float))
                    {
                        float newValue = EditorGUILayout.FloatField((float)value);
                        field.SetValue(script.testData, newValue);
                    }
                    else if (fieldType == typeof(string))
                    {
                        string newValue = EditorGUILayout.TextField((string)value);
                        field.SetValue(script.testData, newValue);
                    }

                    EditorGUILayout.EndHorizontal();
                }
            }

            if (GUI.changed)
            {
                EditorUtility.SetDirty(target);
            }
        }
    }
}

