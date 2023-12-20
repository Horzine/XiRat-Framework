using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Xi.Config.Editor
{
    public static class ConfigDataGenerateTool
    {
        private static readonly Dictionary<string, Type> typeCache = new();

        public static void GenerateCode(string filePath, string outputFilePath)
        {
            var lines = new List<string>(File.ReadAllLines(filePath));
            string[] memberNames = lines[1].Split('\t');
            string[] memberTypes = lines[2].Split('\t');
            string code = GenerateClassCode(Path.GetFileNameWithoutExtension(outputFilePath), memberNames, memberTypes);
            File.WriteAllText(outputFilePath, code);
        }
        private static string GenerateClassCode(string className, string[] memberNames, string[] memberTypes)
        {
            var sb = new StringBuilder();
            sb.AppendLine(@"//
// This code is Generated. Do not modify !
//");
            sb.AppendLine("using System.Collections.Generic;");
            sb.AppendLine("using Newtonsoft.Json.Linq;\n");
            sb.AppendLine($@"namespace Xi.Config
{{
    [{$"{nameof(ConfigDataTypeAttribute).Replace("Attribute", "")}"}]
    public class {className} : IConfigData
    {{
        string IConfigData.Key => {memberNames[0]};");

            for (int i = 0; i < memberNames.Length; i++)
            {
                string memberName = memberNames[i];
                string memberType = memberTypes[i];

                sb.AppendLine($@"        public {memberType} {memberName} {{ get; private set; }}");
            }

            sb.AppendLine(@"    }
}");
            return sb.ToString();
        }
    }
}
