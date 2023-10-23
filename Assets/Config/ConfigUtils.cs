using System;
using System.Collections.Generic;
using System.Reflection;

namespace XiConfig
{
    public class ConfigUtils
    {
        public const string kTxtOriginFolder = "Assets/Config/Origin";
        public const string kCSharpOutputFolder = "Assets/Config/Output";
        public const string kRuntimLoadFolder = "Assets/Config/Origin";

        private static readonly Dictionary<string, Type> typeCache = new();

        public static void ParseConfigData<T>(string[] lines, Dictionary<string, T> resultDic) where T : IConfigData, new()
        {
            string[] memberNames = lines[1].Split('\t');
            string[] memberTypes = lines[2].Split('\t');
            for (int i = 3; i < lines.Length; i++)
            {
                if (string.IsNullOrWhiteSpace(lines[i]))
                {
                    continue;
                }

                var unit = new T();
                string[] values = lines[i].Split("\t");
                for (int j = 0; j < values.Length; j++)
                {
                    SetMemberValue(unit, memberNames[j], memberTypes[j], values[j]);
                }

                resultDic.Add(unit.Key, unit);
            }
        }
        public static void SetMemberValue(object obj, string memberName, string typeName, string value)
        {
            var type = obj.GetType();
            var field = type.GetProperty(memberName);
            string typeNameStr = GetSystemName(typeName);
            var valueType = Type.GetType(typeNameStr);
            if (valueType != null && valueType.IsGenericType && valueType.GetGenericTypeDefinition() == typeof(List<>))
            {
                string[] valueArr = value.TrimStart('[').TrimEnd(']').Split(',');
                var listType = valueType.GetGenericArguments()[0];

                var method = typeof(ConfigUtils).GetMethod("SetValueToList", BindingFlags.NonPublic | BindingFlags.Static);
                var genericMethod = method.MakeGenericMethod(listType);

                genericMethod.Invoke(null, new object[] { obj, field, valueArr });
            }
            else
            {
                object parsedValue = Convert.ChangeType(value, valueType);
                field.SetValue(obj, parsedValue);
            }
        }
        private static void SetValueToList<T>(object obj, PropertyInfo field, string[] valueArr)
        {
            var list = new List<T>();
            foreach (string item in valueArr)
            {
                list.Add((T)Convert.ChangeType(item, typeof(T)));
            }

            field.SetValue(obj, list);
        }
        public static string GetSystemName(string typeName)
        {
            if (typeCache.ContainsKey(typeName))
            {
                return typeCache[typeName].FullName;
            }

            var type = typeName switch
            {
                "string" => typeof(string),
                "int" => typeof(int),
                "float" => typeof(float),
                "bool" => typeof(bool),
                "List<string>" => typeof(List<string>),
                "List<int>" => typeof(List<int>),
                "List<float>" => typeof(List<float>),
                "List<bool>" => typeof(List<bool>),
                _ => null,
            };

            if (type != null)
            {
                typeCache[typeName] = type;
                return type.FullName;
            }

            return null;
        }
    }
}