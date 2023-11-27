using System.Collections.Generic;
using UnityEngine;
using System.Text;
using System.Linq;
using System.IO;
using System;
using Xi.Config;
using Xi.Extend.Attribute;
using Xi.Tools;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Xi.Data
{
    [Serializable]
    public class EntryInfo
    {
        public int configId;
        public SoCollectionEntry_SO entry_so;
    }
    [CreateAssetMenu(fileName = "SoCollection_SO_", menuName = "Xi/Data/ScriptableObject Collection")]
    public class SoCollection_SO : ScriptableObject
    {
        [ReadOnly, SerializeField] private int m_NextConfigId = 1;
        [SerializeField] private string m_ExportTxtFileName;
        [SerializeField] private List<EntryInfo> m_So_Info;
        public string FileOutputFullName => $"{ConfigUtils.kTxtOriginFolder}/{ExportTxtFileName}{ConfigUtils.kConfigFileSuffix}";
        public string ExportTxtFileName => m_ExportTxtFileName;
        public static bool ConfigIdValid(int id) => id > 0;

#if UNITY_EDITOR

        private void CollectionAllEntry()
        {
            m_So_Info.Clear();
            string thisPath = AssetDatabase.GetAssetPath(this);
            var dirInfo = new DirectoryInfo(thisPath).Parent;
            var allPath = dirInfo.GetFiles();
            foreach (var item in allPath)
            {
                string absolutepath = item.FullName.Replace('\\', '/');
                if (!absolutepath.StartsWith(Application.dataPath))
                {
                    continue;
                }

                string relativepath = $"Assets{absolutepath[Application.dataPath.Length..]}";
                var entryObj = AssetDatabase.LoadAssetAtPath<SoCollectionEntry_SO>(relativepath);
                if (entryObj != null)
                {
                    m_So_Info.Add(new EntryInfo
                    {
                        configId = entryObj.GetIntConfigId(true),
                        entry_so = entryObj
                    });
                }
            }

            EditorUtility.SetDirty(this);
            AssetDatabase.SaveAssetIfDirty(this);
        }
        private void CheckItemConfigId()
        {
            foreach (var item in m_So_Info)
            {
                if (!ConfigIdValid(item.configId))
                {
                    item.configId = m_NextConfigId;
                    item.entry_so.SetIntConfigId(item.configId);
                    m_NextConfigId += 1;
                    EditorUtility.SetDirty(item.entry_so);
                    AssetDatabase.SaveAssetIfDirty(item.entry_so);
                }
            }

            m_So_Info.Sort((x, y) => x.configId.CompareTo(y.configId));

            XiLogger.Log($"General Item ConfigId success");
        }
        private void ExportTxtFile()
        {
            if (m_So_Info.Count == 0)
            {
                XiLogger.LogWarning("List is empty");
                return;
            }

            if (string.IsNullOrEmpty(ExportTxtFileName))
            {
                XiLogger.LogError($"m_ExportTxtFileName is Empty");
                return;
            }

            var first = m_So_Info.First();
            var sb = new StringBuilder();
            sb.AppendLine(first.entry_so.ToTxt_Comment());
            sb.AppendLine(first.entry_so.ToTxt_Header());
            sb.AppendLine(first.entry_so.ToTxt_Type());
            sb.AppendLine();

            foreach (var item in m_So_Info)
            {
                sb.AppendLine(item.entry_so.ToTxt_Entry());
            }

            File.WriteAllText(FileOutputFullName, sb.ToString());
            XiLogger.Log($"Export File Success! Path: {FileOutputFullName}");
        }
        public void DoExport()
        {
            CollectionAllEntry();
            if (m_So_Info.Count == 0)
            {
                XiLogger.LogWarning($"No So_Info Entry");
                return;
            }

            CheckItemConfigId();
            ExportTxtFile();
        }
#endif
    }

#if UNITY_EDITOR

    [CustomEditor(typeof(SoCollection_SO))]
    public class SoCollection_SO_Editor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            var script = (SoCollection_SO)target;

            if (GUILayout.Button($"Export '{script.ExportTxtFileName}{ConfigUtils.kConfigFileSuffix}' File", GUILayout.Height(20)))
            {
                script.DoExport();
                AssetDatabase.Refresh();
            }

            if (GUILayout.Button($"Open '{script.ExportTxtFileName}{ConfigUtils.kConfigFileSuffix}' File", GUILayout.Height(20)))
            {
                EditorGUIUtility.PingObject(AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(script.FileOutputFullName));
                System.Diagnostics.Process.Start(Path.GetFullPath(script.FileOutputFullName));
            }
        }
    }
#endif
}

