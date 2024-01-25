using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;
using Type = System.Type;

namespace Xi.Config.Editor
{
    public static class ConfigCollectionGenerateTool
    {
        private const string kConfigCollectionTemplateTextPath = "Assets/Config/Editor/Template Text/ConfigCollectionTemplate.txt";

        public static void GenerateCode(string outputFilePath)
        {
            var configDataTypes = GetConfigDataTypes();
            string code = GenerateCollectionCode(configDataTypes);
            File.WriteAllText(outputFilePath, code);
            AssetDatabase.ImportAsset(outputFilePath);
        }

        private static List<Type> GetConfigDataTypes()
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            var assembly = assemblies.FirstOrDefault(item => item.FullName.Contains("Config,"));
            if (assembly == null)
            {
                Debug.LogError("Config assembly not found.");
                return new List<Type>();
            }

            var types = assembly.GetTypes();
            return types.Where(type => Attribute.IsDefined(type, typeof(ConfigDataTypeAttribute)))
                        .ToList();
        }

        private static string GenerateCollectionCode(List<Type> configDataTypes)
        {
            string template = File.ReadAllText(kConfigCollectionTemplateTextPath);
            var sb = new StringBuilder();
            foreach (var type in configDataTypes)
            {
                var typeName = type.Name;
                sb.AppendLine($"            ConfigUtils.LoadAndParseConfigDictionary(\"{typeName}\", ref {ConfigDataGenerateTool.ClassNameParseToFieldName(typeName)});");
            }
            return template.Replace("{CONTENT}", sb.ToString());
        }
    }
}
