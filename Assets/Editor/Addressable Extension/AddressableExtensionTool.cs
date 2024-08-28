using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEditor.AddressableAssets;
using UnityEditor.AddressableAssets.Settings;
using UnityEditor.AddressableAssets.Settings.GroupSchemas;
using UnityEngine;
using Xi.Framework;
using Xi.Tools;

namespace Xi.EditorExtension
{
    public static class AddressableExtendTool
    {
        private const string kShouldBuildSceneFolderPath = "Assets/Scenes/Build";
        private const string kAutoGroupingFloderConfigPath = "Assets/Editor/Addressable Extend/AutoGroupingFloderConfig.asset";

        [MenuItem("Xi-Tool/Addressable Extend Tool/Rename Address by FileName")]
        public static void RenameAddressByFileName()
        {
            var settings = AddressableAssetSettingsDefaultObject.Settings;
            if (settings != null)
            {
                var groups = settings.groups;
                foreach (var group in groups)
                {
                    var entries = new List<AddressableAssetEntry>(group.entries);
                    foreach (var entry in entries)
                    {
                        string oldAddress = entry.address;
                        string newAddress = string.Concat(entry.parentGroup.name, "/", Path.GetFileNameWithoutExtension(entry.AssetPath));
                        if (oldAddress != newAddress)
                        {
                            entry.SetAddress(newAddress, false);
                            XiLogger.Log(string.Concat("Renamed Address: ", oldAddress, " to ", newAddress));
                        }
                    }
                }

                settings.SetDirty(AddressableAssetSettings.ModificationEvent.EntryMoved, null, true, true);

            }

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            XiLogger.Log("Rename Address By File Name Finish");
        }

        [MenuItem("Xi-Tool/Addressable Extend Tool/Auto Grouping Folder")]
        public static void AutoGroupingAllFolder()
        {
            var config = LoadOrCreateConfig();
            var autoGroupingFolders = config.GetFloderPaths();

            if (autoGroupingFolders.Count == 0)
            {
                XiLogger.LogWarning($"{nameof(AutoGroupingFloderConfig)}.floders is Empty !");
                return;
            }

            foreach (string item in autoGroupingFolders)
            {
                DoAutoGroupingFolder(item);
            }

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            XiLogger.Log("Auto Grouping All Folder Finish");
        }
        private static void DoAutoGroupingFolder(string folderPath)
        {
            var settings = AddressableAssetSettingsDefaultObject.Settings;
            string[] subFolders = AssetDatabase.GetSubFolders(folderPath);
            string parentFolder = Path.GetFileName(folderPath);
            string groupStartWith = string.Concat(parentFolder, "-");
            var defaultSchemas = settings.DefaultGroup.Schemas;

            foreach (string subFolder in subFolders)
            {
                string folderName = Path.GetFileName(subFolder);
                string groupName = string.Concat(groupStartWith, folderName);
                var group = settings.FindGroup(groupName);
                if (group)
                {
                    settings.RemoveGroup(group);
                }

                group = settings.CreateGroup(groupName, false, false, false, defaultSchemas);
                var entry = settings.CreateOrMoveEntry(AssetDatabase.AssetPathToGUID(subFolder), group);
                entry.SetAddress(string.Concat(group.name, "/", folderName), false);
            }

            foreach (var item in settings.groups)
            {
                if (item.name.StartsWith(groupStartWith) && item.entries.Count == 0)
                {
                    settings.RemoveGroup(item);
                }
            }
        }
        private static AutoGroupingFloderConfig LoadOrCreateConfig()
        {
            var config = AssetDatabase.LoadAssetAtPath<AutoGroupingFloderConfig>(kAutoGroupingFloderConfigPath);
            if (config == null)
            {
                config = ScriptableObject.CreateInstance<AutoGroupingFloderConfig>();
                AssetDatabase.CreateAsset(config, kAutoGroupingFloderConfigPath);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
                XiLogger.Log($"New AutoGroupingFloderConfig created at {kAutoGroupingFloderConfigPath}");
            }

            return config;
        }

        [MenuItem("Xi-Tool/Addressable Extend Tool/Auto Grouping Scene")]
        public static void AutoGroupingScene()
        {
            var settings = AddressableAssetSettingsDefaultObject.Settings;
            string groupName = AssetGroupNameConst.kAddressableGroupName_Scene;
            var group = settings.FindGroup(groupName);
            if (group)
            {
                settings.RemoveGroup(group);
            }

            group = settings.CreateGroup(groupName, false, false, false, settings.DefaultGroup.Schemas);
            var schema = group.GetSchema<BundledAssetGroupSchema>();
            schema.BundleMode = BundledAssetGroupSchema.BundlePackingMode.PackSeparately;

            string[] scenePaths = AssetDatabase.FindAssets("t:Scene", new string[] { kShouldBuildSceneFolderPath }).Select(AssetDatabase.GUIDToAssetPath).ToArray();

            foreach (string scenePath in scenePaths)
            {
                var scene = AssetDatabase.LoadAssetAtPath<SceneAsset>(scenePath);
                var entry = settings.CreateOrMoveEntry(AssetDatabase.AssetPathToGUID(scenePath), group);
                entry.SetAddress(SceneNameConst_Extension.SceneAddressableName(scene.name), false);
            }

            settings.SetDirty(AddressableAssetSettings.ModificationEvent.GroupAdded, group, true, true);

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            XiLogger.Log("Auto Grouping Scene Finish");
        }

        [MenuItem("Xi-Tool/Addressable Extend Tool/Manage Groups")]
        public static void ManageGroups()
        {
            string assemblyName = "Unity.Addressables.Editor";
            var type = Assembly.Load(assemblyName)
                                .GetTypes()
                                .FirstOrDefault(t => t.FullName == "UnityEditor.AddressableAssets.GUI.AddressableAssetsWindow");
            if (type != null)
            {
                var method = type.GetMethod("Init", BindingFlags.Static | BindingFlags.NonPublic);
                method?.Invoke(null, null);
            }
        }
    }
}