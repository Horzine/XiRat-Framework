using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace Xi.Config
{
    public class ConfigUtils
    {
        public const string kTxtOriginFolder = "Assets/Config/Origin";
        public const string kCSharpOutputFolder = "Assets/Config/Output";
        public const string kSerializeFolderName = "SerializeData";
        public static readonly string kRuntimeLoadPath = Path.Combine(Application.streamingAssetsPath, kSerializeFolderName);
        public static readonly string kSerializeDataFolderPath = Path.Combine("Assets/StreamingAssets", kSerializeFolderName);
        public const string kOriginConfigFileSuffix = ".txt";
        public const string kKey = "LoveXiForever";

        private static readonly Dictionary<string, Type> typeCache = new();

        private static void ParseConfigData<T>(string[] lines, Dictionary<string, T> resultDic) where T : IConfigData, new()
        {
            string memberNamesLine = lines[1];
            string memberTypesLine = lines[2];
            string[] memberNames = memberNamesLine.Split('\t');
            string[] memberTypes = memberTypesLine.Split('\t');
            if (memberNames.Length != memberTypes.Length)
            {
                Debug.LogError($"[{nameof(ConfigUtils)}] <{nameof(ParseConfigData)}> ===> memberNames.Length != memberTypes.Length, memberNames = {memberNamesLine}, memberTypes = {memberTypesLine}");
                return;
            }

            resultDic = new();

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
        private static void SetMemberValue(object obj, string memberName, string typeName, string value)
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
        private static string GetTypeDefineName(string typeName)
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
        private static string[] DeserializeFromFile(string inputPath, string key)
        {
            if (key.Length < 32)
            {
                key = key.PadRight(32, 'X');
            }

            using var fs = new FileStream(inputPath, FileMode.Open);
            using var aesAlg = new AesCryptoServiceProvider();
            aesAlg.Key = Encoding.UTF8.GetBytes(key);
            aesAlg.IV = new byte[16]; // Use zero IV for simplicity; in practice, retrieve the IV from the encrypted file.

            using var decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);
            using var cryptoStream = new CryptoStream(fs, decryptor, CryptoStreamMode.Read);
            using var reader = new StreamReader(cryptoStream, Encoding.UTF8);

            var decryptedLines = new List<string>();
            while (!reader.EndOfStream)
            {
                decryptedLines.Add(reader.ReadLine());
            }

            return decryptedLines.ToArray();
        }
        public static void SerializeToFile(string[] lines, string outputPath, string key)
        {
            if (key.Length < 32)
            {
                key = key.PadRight(32, 'X');
            }

            using var fs = new FileStream(outputPath, FileMode.Create);
            using var aesAlg = new AesCryptoServiceProvider();
            aesAlg.Key = Encoding.UTF8.GetBytes(key);
            aesAlg.IV = new byte[16]; // Use zero IV for simplicity; in practice, generate a random IV for each encryption.

            using var encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);
            using var cryptoStream = new CryptoStream(fs, encryptor, CryptoStreamMode.Write);
            using var writer = new StreamWriter(cryptoStream, Encoding.UTF8);

            foreach (string line in lines)
            {
                writer.WriteLine(line);
            }
        }

        public static void LoadAndParseConfigDictionary<TConfigData>(string fileName, ref Dictionary<string, TConfigData> dataDic)
            where TConfigData : IConfigData, new()
        {
#if UNITY_EDITOR
            string[] lines = File.ReadAllLines(Path.Combine(kTxtOriginFolder, $"{fileName}{kOriginConfigFileSuffix}"));
#else
            string[] lines = DeserializeFromFile(Path.Combine(kRuntimeLoadPath, fileName), kKey);
#endif
            ParseConfigData(lines, dataDic);
        }
    }
}