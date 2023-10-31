using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.AddressableAssets;
using UnityEditor.AddressableAssets.Settings;
using UnityEngine;

namespace Xi.EditorExtend
{
    public class AddressableExtendTool
    {
        private static readonly List<string> _folderPath = new()
        {// Folder path here 

        };

        [MenuItem("Xi/Addressable Extend Tool/Auto Grouping Folder")]
        public static void AutoGroupingAllFolder()
        {
            foreach (string item in _folderPath)
            {
                DoAutoGroupingFolder(item);
            }

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
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
                if (!group)
                {
                    group = settings.CreateGroup(groupName, false, false, false, defaultSchemas);
                }

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

        [MenuItem("Xi/Addressable Extend Tool/Rename Address by FileName")]
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
                            Debug.Log(string.Concat("Renamed Address: ", oldAddress, " to ", newAddress));
                        }
                    }
                }

                settings.SetDirty(AddressableAssetSettings.ModificationEvent.EntryMoved, null, true, true);

            }

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
    }
}