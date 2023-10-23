using System.IO;
using UnityEditor;

namespace Config.Editor
{
    public static class ConfigToolEditor
    {
        private const string kTxtOriginFolder = ConfigUtils.kTxtOriginFolder;
        private const string kCSharpOutputFolder = ConfigUtils.kCSharpOutputFolder;

        [MenuItem("ConfigTool/Generate ConfigData")]
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
                string originPath = Path.Combine(kTxtOriginFolder, fileName + ".txt");
                string outputPath = Path.Combine(kCSharpOutputFolder, fileName + ".cs");

                ConfigDataGenerateTool.GenerateCode(originPath, outputPath);
                AssetDatabase.ImportAsset(outputPath);
            }

            UnityEditor.Compilation.CompilationPipeline.RequestScriptCompilation();
            AssetDatabase.Refresh();
        }

        [MenuItem("ConfigTool/Generate ConfigCollection")]
        public static void GenerateConfigCollection()
        {
            if (!Directory.Exists(kCSharpOutputFolder))
            {
                Directory.CreateDirectory(kCSharpOutputFolder);
            }

            string outputFilePath = Path.Combine(kCSharpOutputFolder, "ConfigCollection.cs");
            ConfigCollectionGenerateTool.GenerateCode(outputFilePath);
            UnityEditor.Compilation.CompilationPipeline.RequestScriptCompilation();
            AssetDatabase.Refresh();
        }

        private static string[] GetTextFiles(string folderPath)
        {
            string[] files = Directory.GetFiles(folderPath, "*.txt", SearchOption.AllDirectories);
            if (files.Length > 0)
            {
                return files;
            }
            else
            {
                UnityEngine.Debug.LogWarning("No text files found.");
                return null;
            }
        }
    }
}
