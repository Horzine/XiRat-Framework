using System.IO;
using UnityEditor;
using UnityEngine;

namespace Xi.Config.Editor
{
    public static class ConfigToolEditor
    {
        private const string kTxtOriginFolder = ConfigUtils.kTxtOriginFolder;
        private const string kCSharpOutputFolder = ConfigUtils.kCSharpOutputFolder;
        private const string kGenerateConfigDataChanged = "GenerateConfigDataChanged";
        private static readonly string kSerializeDataFolderPath = ConfigUtils.kSerializeDataFolderPath;

        [MenuItem("Xi/Config Tool/Generate ConfigData C#")]
        public static void GenerateAllConfigDataCSharp()
        {
            if (!Directory.Exists(kCSharpOutputFolder))
            {
                Directory.CreateDirectory(kCSharpOutputFolder);
            }

            if (!Directory.Exists(kTxtOriginFolder))
            {
                Directory.CreateDirectory(kTxtOriginFolder);
            }

            string[] textFiles = GetTextFiles(kTxtOriginFolder);
            foreach (string item in textFiles)
            {
                string fileName = Path.GetFileNameWithoutExtension(item);
                string originPath = Path.Combine(kTxtOriginFolder, string.Concat(fileName, ConfigUtils.kOriginConfigFileSuffix));
                string outputPath = Path.Combine(kCSharpOutputFolder, string.Concat(fileName, ".cs"));

                ConfigDataGenerateTool.GenerateCode(originPath, outputPath);
                AssetDatabase.ImportAsset(outputPath);
            }

            Debug.Log($"[{nameof(ConfigToolEditor)}] <{nameof(GenerateAllConfigDataCSharp)}> ===> Finish! Files:\n{string.Join('\n', textFiles)}");
            AssetDatabase.Refresh();
            UnityEditor.Compilation.CompilationPipeline.RequestScriptCompilation();

            EditorPrefs.SetBool(kGenerateConfigDataChanged, true);
        }

        [MenuItem("Xi/Config Tool/Generate ConfigCollection")]
        public static void GenerateConfigCollection()
        {
            if (!Directory.Exists(kCSharpOutputFolder))
            {
                Directory.CreateDirectory(kCSharpOutputFolder);
            }

            string outputFilePath = Path.Combine(kCSharpOutputFolder, "ConfigCollection.cs");
            ConfigCollectionGenerateTool.GenerateCode(outputFilePath);

            Debug.Log($"[{nameof(ConfigToolEditor)}] <{nameof(GenerateConfigCollection)}> ===> Finish! File: {outputFilePath}");
            AssetDatabase.Refresh();
            UnityEditor.Compilation.CompilationPipeline.RequestScriptCompilation();
        }

        private static string[] GetTextFiles(string folderPath)
        {
            string[] files = Directory.GetFiles(folderPath, $"*{ConfigUtils.kOriginConfigFileSuffix}", SearchOption.AllDirectories);
            if (files.Length > 0)
            {
                return files;
            }
            else
            {
                Debug.LogWarning($"[{nameof(ConfigToolEditor)}] <{nameof(GetTextFiles)}> ===> No text files found.");
                return null;
            }
        }

        [UnityEditor.Callbacks.DidReloadScripts]
        private static void OnScriptsReloaded()
        {
            bool isConfigDataChanged = EditorPrefs.GetBool(kGenerateConfigDataChanged, false);
            if (isConfigDataChanged)
            {
                EditorPrefs.SetBool(kGenerateConfigDataChanged, false);
                GenerateConfigCollection();
                SerializeAllData();
            }
        }

        [MenuItem("Xi/Config Tool/Serialize All Data")]
        public static void SerializeAllData()
        {
            if (!Directory.Exists(kTxtOriginFolder))
            {
                Debug.LogWarning($"[{nameof(ConfigToolEditor)}] <{nameof(SerializeAllData)}> ===> '{kTxtOriginFolder}' not Exists");
                return;
            }

            if (Directory.Exists(kSerializeDataFolderPath))
            {
                Directory.Delete(kSerializeDataFolderPath, true);
            }

            Directory.CreateDirectory(kSerializeDataFolderPath);

            string[] cfgFiles = GetTextFiles(kTxtOriginFolder);
            foreach (string item in cfgFiles)
            {
                string fileName = Path.GetFileNameWithoutExtension(item);
                ConfigUtils.SerializeToFile(File.ReadAllLines(item), Path.Combine(kSerializeDataFolderPath, fileName), ConfigUtils.kKey);
            }

            Debug.Log($"[{nameof(ConfigToolEditor)}] <{nameof(SerializeAllData)}> ===> Finish! File:\n{string.Join('\n', cfgFiles)}");
            AssetDatabase.Refresh();
        }
    }
}
