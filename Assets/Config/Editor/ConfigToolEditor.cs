using System;
using System.IO;
using UnityEditor;

namespace Xi.Config.Editor
{
    public static class ConfigToolEditor
    {
        private const string kTxtOriginFolder = ConfigUtils.kTxtOriginFolder;
        private const string kCSharpOutputFolder = ConfigUtils.kCSharpOutputFolder;
        private const string kGenerateConfigDataChanged = "GenerateConfigDataChanged";

        [MenuItem("Xi/Config Tool/Generate ConfigData")]
        public static void GenerateAllConfigData()
        {
            if (!Directory.Exists(kCSharpOutputFolder))
            {
                Directory.CreateDirectory(kCSharpOutputFolder);
            }

            if (!Directory.Exists(kTxtOriginFolder))
            {
                Directory.CreateDirectory(kTxtOriginFolder);
            }

            foreach (string item in GetTextFiles(kTxtOriginFolder))
            {
                string fileName = Path.GetFileNameWithoutExtension(item);
                string originPath = Path.Combine(kTxtOriginFolder, string.Concat(fileName, ConfigUtils.kOriginConfigFileSuffix));
                string outputPath = Path.Combine(kCSharpOutputFolder, string.Concat(fileName, ".cs"));

                ConfigDataGenerateTool.GenerateCode(originPath, outputPath);
                AssetDatabase.ImportAsset(outputPath);
            }

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
                UnityEngine.Debug.LogWarning($"[{nameof(ConfigToolEditor)}] <{nameof(GetTextFiles)}> ===> No text files found.");
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
            }

            SerializeAllData();
        }

        [MenuItem("Xi/Config Tool/Serialize All Data")]
        public static void SerializeAllData()
        {
            string folderPath = ConfigUtils.kTxtOriginFolder;

            if (!Directory.Exists(folderPath))
            {
                // Log Waring
                return;
            }

            if (Directory.Exists(ConfigUtils.kSerializeDataFolderPath))
            {
                Directory.Delete(ConfigUtils.kSerializeDataFolderPath, true);
            }

            Directory.CreateDirectory(ConfigUtils.kSerializeDataFolderPath);

            string[] cfgFiles = Array.FindAll(Directory.GetFiles(folderPath), s => s.EndsWith(ConfigUtils.kOriginConfigFileSuffix));
            foreach (string item in cfgFiles)
            {
                string fileName = Path.GetFileNameWithoutExtension(item);
                ConfigUtils.SerializeToFile(File.ReadAllLines(item), Path.Combine(ConfigUtils.kSerializeDataFolderPath, fileName), ConfigUtils.kKey);
            }

            AssetDatabase.Refresh();
        }
    }
}
