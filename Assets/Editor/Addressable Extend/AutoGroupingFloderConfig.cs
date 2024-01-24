using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using Xi.Tools;

namespace Xi.EditorExtend
{
    [CreateAssetMenu(fileName = "AutoGroupingFloderConfig", menuName = "Xi/AutoGroupingFloderConfig")]
    public class AutoGroupingFloderConfig : ScriptableObject
    {
        public Object[] floders = new Object[0];

        public List<string> GetFloderPaths()
        {
            var result = new List<string>();
            foreach (var obj in floders)
            {
                if (obj != null)
                {
                    string assetPath = AssetDatabase.GetAssetPath(obj);

                    if (Directory.Exists(assetPath))
                    {
                        // 是文件夹，执行你的后续代码
                        XiLogger.Log("Folder Asset Path: " + assetPath);
                        result.Add(assetPath);
                    }
                }
            }

            return result;
        }
    }
}
