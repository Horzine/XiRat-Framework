﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace Xi.Framework.Editor
{
    public static class EventIdGeneratorTool
    {
        private const string kTemplatePath = "Assets/Scripts/Framework/Event Center/Editor/GenerateCustomEventIdTemplate.txt";
        private const string kOutputPath = "Assets/Scripts/Framework/Event Center/AutoGeneratedCustomEventId.cs";

        [MenuItem("Xi-Tool/Generate Custom Event Id")]
        public static void GenerateCustomEventId()
        {
            var baseType = typeof(CustomEvent);
            var eventTypes = new List<Type>();
            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                foreach (var type in assembly.GetTypes())
                {
                    if (type.IsSubclassOf(baseType) && !type.IsAbstract)
                    {
                        eventTypes.Add(type);
                    }
                }
            }

            GenerateCodeFromTemplate(kTemplatePath, kOutputPath, eventTypes);

            Debug.Log($"[{nameof(EventIdGeneratorTool)}] <{nameof(GenerateCustomEventId)}> ===> Finish! Files at: {kOutputPath}");
        }

        private static void GenerateCodeFromTemplate(string templatePath, string outputPath, List<Type> eventTypes)
        {
            string templateContent = File.ReadAllText(templatePath);

            var typeMapIntBuilder = new StringBuilder();
            var intMapTypeBuilder = new StringBuilder();
            var preMap = CustomEventDefine.TypeNameMapInt;
            int nextEventId = CustomEventDefine.nextEventId;
            var intMapType = new Dictionary<int, Type>();
            var typeMapInt = new Dictionary<Type, int>();

            foreach (var eventType in eventTypes)
            {
                string eventName = eventType.Name;
                string fullName = eventType.FullName;
                if (preMap.TryGetValue(fullName, out int enumValue))
                {
                    intMapType.Add(enumValue, eventType);
                    typeMapInt.Add(eventType, enumValue);
                }
                else
                {
                    intMapType.Add(nextEventId, eventType);
                    typeMapInt.Add(eventType, nextEventId);
                    nextEventId++;
                }
            }

            var intList = intMapType.Keys.ToList();
            intList.Sort();
            foreach (int id in intList)
            {
                string fullName = intMapType[id].FullName;
                typeMapIntBuilder.AppendLine($"            {{ \"{fullName}\", {id} }},");
                intMapTypeBuilder.AppendLine($"            {{ {id}, \"{fullName}\" }},");
            }

            var inheritanceChainMap = new SortedList<int, int[]>();
            var tempChainCache = new List<int>();
            foreach (int id in intList)
            {
                var eventType = intMapType[id];
                tempChainCache.Clear();

                var currentType = eventType;
                while (currentType != typeof(CustomEvent))
                {
                    if (typeMapInt.TryGetValue(currentType.BaseType, out int parentId))
                    {
                        tempChainCache.Add(parentId);
                    }

                    currentType = currentType.BaseType;
                }

                inheritanceChainMap.Add(id, tempChainCache.ToArray());
            }

            var inheritanceChainMapBuilder = new StringBuilder();
            foreach (var item in inheritanceChainMap)
            {
                inheritanceChainMapBuilder.AppendLine($"            {{ {item.Key}, new int[] {{ {string.Join(", ", item.Value)} }} }},");
            }

            string typeMapIntItems = typeMapIntBuilder.ToString().TrimEnd();
            string intMapTypeItems = intMapTypeBuilder.ToString().TrimEnd();
            string inheritanceChains = inheritanceChainMapBuilder.ToString().TrimEnd();
            string outputContent = templateContent
                .Replace("%NEXT_EVENT_ID%", nextEventId.ToString())
                .Replace("%TYPE_NAME_MAP_INT%", typeMapIntItems)
                .Replace("%INT_MAP_TYPE_NAME%", intMapTypeItems)
                .Replace("%INHERITANCE_CHAIN_MAP%", inheritanceChains);

            File.WriteAllText(outputPath, outputContent);
        }
    }
}
