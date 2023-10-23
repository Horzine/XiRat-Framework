using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Type = System.Type;

namespace Config.Editor
{
    public static class ConfigCollectionGenerateTool
    {
        public static void GenerateCode(string outputFilePath)
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            var assembly = Array.Find(assemblies, item => item.FullName.Contains("Assembly-CSharp"));
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
        }
        private static string GenerateCollectionCode(string className, List<Type> configDataTypes)
        {
            var sb = new StringBuilder();

            sb.AppendLine(@$"//
// This code is Generated. Do not modify !
//
using System.Collections.Generic;
using System.IO;

namespace Config
{{
    public class {className}
    {{
        private const string kLoadFloder = ConfigUtils.kRuntimLoadFolder;");

            foreach (var item in configDataTypes)
            {
                sb.AppendLine($"        public Dictionary<string, {item.Name}> All{item.Name} {{ get; }} = new();");
            }

            sb.AppendLine(@"
        public void Init()
        {");

            foreach (var item in configDataTypes)
            {
                sb.AppendLine($@"            ConfigUtils.ParseConfigData(File.ReadAllLines(Path.Combine(kLoadFloder, ""{item.Name}.txt"")), All{item.Name});");
            }

            sb.AppendLine(@"
        }
    }
}");

            return sb.ToString();
        }
    }
}
