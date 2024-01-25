using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Xi.Config.Editor
{
    public static class ConfigDataGenerateTool
    {
        private const string kClassTemplateTextPath = "Assets/Config/Editor/Template Text/ClassTemplate.txt";

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
            string template = File.ReadAllText(kClassTemplateTextPath);

            template = template.Replace("{CLASS_NAME}", className)
                               .Replace("{KEY_NAME}", memberNames[0])
                               .Replace("{FIELD_NAME}", ClassNameParseToFieldName(className))
                               .Replace("{PROPERTY}", GenerateProperties(memberNames, memberTypes));

            return template;
        }

        private static string GenerateProperties(string[] memberNames, string[] memberTypes)
        {
            var properties = memberNames.Zip(memberTypes, (name, type) => $"        public {type} {name} {{ get; private set; }}");
            return string.Join(Environment.NewLine, properties);
        }

        public static string ClassNameParseToFieldName(string className) => $"_{char.ToLower(className[0])}{className[1..]}";
    }
}
