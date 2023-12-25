using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Xi.Tools;

namespace Xi.Framework
{
    public class UserArchiveManager : MonoSingleton<UserArchiveManager>, ISingleton
    {
        public struct SaveData
        {
            public string systemName;
            public byte[] data;

            public SaveData(string systemName, byte[] data)
            {
                this.systemName = systemName;
                this.data = data;
            }
        }
        private const string kSaveFileName = "SaveData.sav";
        public string FilePath =>
#if UNITY_EDITOR
                Path.Combine(Application.streamingAssetsPath, kSaveFileName);
#else
                Path.Combine(Application.persistentDataPath, kSaveFileName);
#endif

        private CancellationTokenSource _saveCancellationTokenSource;

        public void OnCreate()
        {

        }

        public async UniTask InitAsync() => await LoadAsync();

        public Dictionary<string, SaveData> CachedSaveData { get; private set; }

        public async UniTask SaveAsync(Dictionary<string, SaveData> saveData)
        {
            _saveCancellationTokenSource?.Cancel(); // Cancel previous save operation, if any
            _saveCancellationTokenSource = new CancellationTokenSource();

            try
            {
                await UniTask.RunOnThreadPool(() =>
                {
                    using var fileStream = new FileStream(FilePath, FileMode.Create, FileAccess.Write);
                    foreach (var system in saveData.Values)
                    {
                        _saveCancellationTokenSource.Token.ThrowIfCancellationRequested(); // Check if cancellation is requested

                        // Write system name length as int32 followed by system name as bytes
                        byte[] systemNameBytes = System.Text.Encoding.UTF8.GetBytes(system.systemName);
                        byte[] systemNameLengthBytes = BitConverter.GetBytes(systemNameBytes.Length);
                        fileStream.Write(systemNameLengthBytes, 0, systemNameLengthBytes.Length);

                        fileStream.Write(systemNameBytes, 0, systemNameBytes.Length);

                        // Write system data length as int32 followed by system data
                        byte[] systemDataLengthBytes = BitConverter.GetBytes(system.data.Length);
                        fileStream.Write(systemDataLengthBytes, 0, systemDataLengthBytes.Length);

                        fileStream.Write(system.data, 0, system.data.Length);
                    }
                }, cancellationToken: _saveCancellationTokenSource.Token);
                CachedSaveData = saveData;
            }
            catch (OperationCanceledException)
            {
                XiLogger.Log("Save operation canceled.");
            }
        }

        public async UniTask LoadAsync()
        {
            await UniTask.RunOnThreadPool(() =>
            {
                if (File.Exists(FilePath))
                {
                    var saveData = new Dictionary<string, SaveData>();
                    using var fileStream = new FileStream(FilePath, FileMode.Open, FileAccess.Read);
                    while (fileStream.Position < fileStream.Length)
                    {
                        // Read system name length as int32 followed by system name as bytes
                        byte[] systemNameLengthBytes = new byte[sizeof(int)];
                        fileStream.Read(systemNameLengthBytes, 0, systemNameLengthBytes.Length);
                        int systemNameLength = BitConverter.ToInt32(systemNameLengthBytes, 0);

                        byte[] systemNameBytes = new byte[systemNameLength];
                        fileStream.Read(systemNameBytes, 0, systemNameBytes.Length);
                        string systemName = System.Text.Encoding.UTF8.GetString(systemNameBytes);

                        // Read system data length as int32 followed by system data
                        byte[] systemDataLengthBytes = new byte[sizeof(int)];
                        fileStream.Read(systemDataLengthBytes, 0, systemDataLengthBytes.Length);
                        int systemDataLength = BitConverter.ToInt32(systemDataLengthBytes, 0);

                        byte[] systemData = new byte[systemDataLength];
                        fileStream.Read(systemData, 0, systemData.Length);

                        saveData.Add(systemName, new SaveData(systemName, systemData));
                    }

                    CachedSaveData = saveData;
                }
                else
                {
                    XiLogger.Log($"File not Exists: FilePath = {FilePath}");
                    CachedSaveData = null;
                }
            });
        }
    }
}
