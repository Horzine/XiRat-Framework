using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEditor;
using Type = System.Type;

namespace Xi.Config.Editor
{
    public static class ConfigCollectionGenerateTool
    {
        public static void GenerateCode(string outputFilePath)
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            var assembly = Array.Find(assemblies, item => item.FullName.Contains("Config,"));
            var types = assembly.GetTypes();
            var configDataTypes = new List<Type>();
            foreach (var type in types)
            {
                if (Attribute.IsDefined(type, typeof(ConfigDataTypeAttribute)))
                {
                    configDataTypes.Add(type);
                }
            }

            string code = GenerateCollectionCode(Path.GetFileNameWithoutExtension(outputFilePath), configDataTypes);
            File.WriteAllText(outputFilePath, code);
            AssetDatabase.ImportAsset(outputFilePath);
        }
        private static string GenerateCollectionCode(string className, List<Type> configDataTypes)
        {
            var sb = new StringBuilder();

            sb.AppendLine(@$"//
// This code is Generated. Do not modify !
//
using System.Collections.Generic;
using System.IO;

namespace Xi.Config
{{
    public class {className}
    {{");

            foreach (var item in configDataTypes)
            {
                sb.AppendLine($"        public Dictionary<string, {item.Name}> All{item.Name} {{ get; }} = new();");
            }

            sb.AppendLine(@"
        public void Init()
        {");
            sb.AppendLine("#if UNITY_EDITOR");
            foreach (var item in configDataTypes)
            {
                sb.AppendLine($@"            ConfigUtils.ParseConfigData(File.ReadAllLines(Path.Combine(ConfigUtils.kTxtOriginFolder, ""{item.Name}{ConfigUtils.kOriginConfigFileSuffix}"")), All{item.Name});");
            }

            sb.AppendLine("#else");

            foreach (var item in configDataTypes)
            {
                sb.AppendLine($@"            ConfigUtils.ParseConfigData(ConfigUtils.DeserializeFromFile(Path.Combine(ConfigUtils.kRuntimeLoadPath, ""{item.Name}""), ConfigUtils.kKey), All{item.Name});");
            }

            sb.AppendLine("#endif");
            sb.AppendLine(@"
        }
    }
}");

            return sb.ToString();
        }
    }
}
