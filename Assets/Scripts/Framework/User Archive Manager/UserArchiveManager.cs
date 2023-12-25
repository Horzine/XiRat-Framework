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
        public event Action OnDestroyAction;

        public void OnCreate()
        {

        }

        public async UniTask InitAsync() => await LoadAsync();

        public Dictionary<string, SaveData> CachedSaveData { get; private set; }

        public async UniTask SaveAsync(Dictionary<string, SaveData> saveData)
        {
            _saveCancellationTokenSource?.Cancel();
            _saveCancellationTokenSource = new CancellationTokenSource();

            try
            {
                await UniTask.RunOnThreadPool(() =>
                {
                    DoSave(saveData);
                }, cancellationToken: _saveCancellationTokenSource.Token);

                CachedSaveData = saveData;
                XiLogger.Log($"Finish, path = {FilePath}");
            }
            catch (OperationCanceledException)
            {
                XiLogger.Log("Save operation canceled.");
            }
            catch (Exception ex)
            {
                XiLogger.LogError($"Failed: {ex}");
            }
        }

        public void SaveSync(Dictionary<string, SaveData> saveData)
        {
            _saveCancellationTokenSource?.Cancel();
            try
            {
                DoSave(saveData);

                CachedSaveData = saveData;
                XiLogger.Log($"Finish, path = {FilePath}");
            }
            catch (Exception ex)
            {
                XiLogger.LogError($"Failed: {ex}");
            }
        }

        private void DoSave(Dictionary<string, SaveData> saveData)
        {
            using var fileStream = new FileStream(FilePath, FileMode.Create, FileAccess.Write);
            try
            {
                foreach (var system in saveData.Values)
                {
                    _saveCancellationTokenSource?.Token.ThrowIfCancellationRequested();

                    // 将系统名称长度作为int32写入，后跟系统名称的字节
                    byte[] systemNameBytes = System.Text.Encoding.UTF8.GetBytes(system.systemName);
                    byte[] systemNameLengthBytes = BitConverter.GetBytes(systemNameBytes.Length);
                    fileStream.Write(systemNameLengthBytes, 0, systemNameLengthBytes.Length);

                    fileStream.Write(systemNameBytes, 0, systemNameBytes.Length);

                    // 将系统数据长度作为int32写入，后跟系统数据
                    byte[] systemDataLengthBytes = BitConverter.GetBytes(system.data.Length);
                    fileStream.Write(systemDataLengthBytes, 0, systemDataLengthBytes.Length);

                    fileStream.Write(system.data, 0, system.data.Length);
                }
            }
            catch
            {
                throw;
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
                    XiLogger.Log($"Load finish. Path = {FilePath}");
                }
                else
                {
                    XiLogger.Log($"File not Exists: FilePath = {FilePath}");
                    CachedSaveData = null;
                }
            });
        }

        private void OnDestroy()
        {
            OnDestroyAction?.Invoke();
            OnDestroyAction = null;
        }
    }
}
