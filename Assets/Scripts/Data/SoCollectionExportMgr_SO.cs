using System.Collections.Generic;
using UnityEngine;
using XiConfig;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Xi.Data
{
    [CreateAssetMenu(fileName = "SoCollectionExportMgr_SO", menuName = "Xi/Data/ScriptableObject Collection Export Manager")]
    public class SoCollectionExportMgr_SO : ScriptableObject
    {
        public List<SoCollection_SO> so_Collections;
    }

#if UNITY_EDITOR

    [CustomEditor(typeof(SoCollectionExportMgr_SO))]
    public class SoCollectionExportMgr_SO_Editor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            var script = (SoCollectionExportMgr_SO)target;

            if (GUILayout.Button($"Export All {ConfigUtils.kConfigFileSuffix} File", GUILayout.Height(20)))
            {
                foreach (var item in script.so_Collections)
                {
                    item.DoExport();
                }
            }
        }
    }
#endif

}