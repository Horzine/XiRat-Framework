using System;
using System.Collections.Generic;
using System.Reflection;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace Xi.Config
{
    public class ConfigUtils
    {
        public const string kTxtOriginFolder = "Assets/Config/Origin";
        public const string kCSharpOutputFolder = "Assets/Config/Output";
        public const string kRuntimLoadFolder = "Assets/Config/Origin";
        public const string kConfigFileSuffix = ".cfg";

        private static readonly Dictionary<string, Type> typeCache = new();

        public static void ParseConfigData<T>(string[] lines, Dictionary<string, T> resultDic) where T : IConfigData, new()
        {
            var memberNamesLine = lines[1];
            var memberTypesLine = lines[2];
            string[] memberNames = memberNamesLine.Split('\t');
            string[] memberTypes = memberTypesLine.Split('\t');
            if (memberNames.Length != memberTypes.Length)
            {
                Debug.LogError($"[{nameof(ConfigUtils)}] <{nameof(ParseConfigData)}> ===> memberNames.Length != memberTypes.Length, memberNames = {memberNamesLine}, memberTypes = {memberTypesLine}");
                return;
            }

            for (int i = 3; i < lines.Length; i++)
            {
                if (string.IsNullOrWhiteSpace(lines[i]))
                {
                    continue;
                }

                var unit = new T();
                string[] values = lines[i].Split("\t");
                if (values.Length != memberNames.Length)
                {
                    Debug.LogError($"[{nameof(ConfigUtils)}] <{nameof(ParseConfigData)}> ===> values.Length != memberNames.Length, memberNames = {memberNamesLine}, values = {lines[i]}");
                    continue;
                }

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
            string typeDefineName = GetTypeDefineName(typeName);
            var valueType = Type.GetType(typeDefineName);
            if (valueType == null)
            {
                Debug.LogError($"[{nameof(ConfigUtils)}] <{nameof(SetMemberValue)}> ===> valueType is null, obj = {obj}, memberName = {memberName}, typeName = {typeName}, value = {value}, typeDefineName = {typeDefineName}");
                return;
            }

            if (valueType.IsGenericType && valueType.GetGenericTypeDefinition() == typeof(List<>))
            {
                string[] valueArr = value.TrimStart('[').TrimEnd(']').Split(',');
                var listType = valueType.GetGenericArguments()[0];

                var method = typeof(ConfigUtils).GetMethod(nameof(SetValueToList), BindingFlags.NonPublic | BindingFlags.Static);
                var genericMethod = method.MakeGenericMethod(listType);

                genericMethod.Invoke(null, new object[] { obj, field, valueArr });
            }
            else if (valueType == typeof(JObject))
            {
                field.SetValue(obj, JObject.Parse(value));
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
        public static string GetTypeDefineName(string typeName)
        {
            if (typeCache.ContainsKey(typeName))
            {
                var t = typeCache[typeName];
                return $"{t.AssemblyQualifiedName}, {t.FullName}";
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
                "JObject" => typeof(JObject),
                _ => null,
            };

            if (type != null)
            {
                typeCache[typeName] = type;
                return $"{type.AssemblyQualifiedName}, {type.FullName}";
            }

            return null;
        }
    }
}