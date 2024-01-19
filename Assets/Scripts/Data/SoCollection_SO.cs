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
        [ReadOnly, SerializeField] private int _nextConfigId = 1;
        [SerializeField] private string _exportTxtFileName;
        [SerializeField] private List<EntryInfo> _soInfo;
        public string FileOutputFullName => $"{ConfigUtils.kTxtOriginFolder}/{ExportTxtFileName}{ConfigUtils.kOriginConfigFileSuffix}";
        public string ExportTxtFileName => _exportTxtFileName;
        public static bool ConfigIdValid(int id) => id > 0;

#if UNITY_EDITOR

        private void CollectionAllEntry()
        {
            _soInfo.Clear();
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
                    if (!ConfigIdValid(entryObj.GetIntConfigId(true)))
                    {
                        entryObj.SetIntConfigId(_nextConfigId);
                        _nextConfigId += 1;
                        EditorUtility.SetDirty(entryObj);
                        AssetDatabase.SaveAssetIfDirty(entryObj);
                    }

                    _soInfo.Add(new EntryInfo
                    {
                        configId = entryObj.GetIntConfigId(true),
                        entry_so = entryObj
                    });
                }
            }

            _soInfo.Sort((x, y) => x.configId.CompareTo(y.configId));

            EditorUtility.SetDirty(this);
            AssetDatabase.SaveAssetIfDirty(this);
        }

        private void ExportTxtFile()
        {
            if (_soInfo.Count == 0)
            {
                XiLogger.LogWarning("List is empty");
                return;
            }

            if (string.IsNullOrEmpty(ExportTxtFileName))
            {
                XiLogger.LogError($"m_ExportTxtFileName is Empty");
                return;
            }

            var first = _soInfo.First();
            var sb = new StringBuilder();
            sb.AppendLine(first.entry_so.ToTxt_Comment());
            sb.AppendLine(first.entry_so.ToTxt_Header());
            sb.AppendLine(first.entry_so.ToTxt_Type());
            sb.AppendLine();

            foreach (var item in _soInfo)
            {
                sb.AppendLine(item.entry_so.ToTxt_Entry());
            }

            File.WriteAllText(FileOutputFullName, sb.ToString());
            XiLogger.Log($"Export File Success! Path: {FileOutputFullName}");
        }
        public void DoExport()
        {
            CollectionAllEntry();
            if (_soInfo.Count == 0)
            {
                XiLogger.LogWarning($"No So_Info Entry");
                return;
            }

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

            if (GUILayout.Button($"Export '{script.ExportTxtFileName}{ConfigUtils.kOriginConfigFileSuffix}' File", GUILayout.Height(20)))
            {
                script.DoExport();
                AssetDatabase.Refresh();
            }

            if (GUILayout.Button($"Open '{script.ExportTxtFileName}{ConfigUtils.kOriginConfigFileSuffix}' File", GUILayout.Height(20)))
            {
                EditorGUIUtility.PingObject(AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(script.FileOutputFullName));
                System.Diagnostics.Process.Start(Path.GetFullPath(script.FileOutputFullName));
            }
        }
    }
#endif
}

