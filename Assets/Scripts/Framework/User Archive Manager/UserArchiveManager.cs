﻿using System;
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
        public string FilePath { get; private set; }

        private CancellationTokenSource _saveCancellationTokenSource;
        public event Action OnDestroyAction;

        void ISingleton.OnCreate() =>
#if UNITY_EDITOR
            FilePath = Path.Combine(Application.streamingAssetsPath, kSaveFileName);
#else
            FilePath = Path.Combine(Application.persistentDataPath, kSaveFileName);
#endif

        public async UniTask InitAsync() => await LoadAsync();

        public void Init() => DoLoad();

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
                XiLogger.Log($"Finish. Path = {FilePath}");
                _saveCancellationTokenSource = null;
            }
            catch (OperationCanceledException)
            {
                XiLogger.Log("Save operation canceled.");
            }
            catch (Exception ex)
            {
                XiLogger.LogError($"Failed ! {ex}");
            }
        }

        public void SaveSync(Dictionary<string, SaveData> saveData)
        {
            _saveCancellationTokenSource?.Cancel();
            _saveCancellationTokenSource = null;
            try
            {
                DoSave(saveData);

                CachedSaveData = saveData;
                XiLogger.Log($"Finish. Path = {FilePath}");
            }
            catch (Exception ex)
            {
                XiLogger.LogError($"Failed ! {ex}");
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

                    byte[] systemNameBytes = System.Text.Encoding.UTF8.GetBytes(system.systemName);
                    byte[] systemNameLengthBytes = BitConverter.GetBytes(systemNameBytes.Length);
                    fileStream.Write(systemNameLengthBytes, 0, systemNameLengthBytes.Length);

                    fileStream.Write(systemNameBytes, 0, systemNameBytes.Length);

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

        private async UniTask LoadAsync() => await UniTask.RunOnThreadPool(DoLoad);

        private void DoLoad()
        {
            if (File.Exists(FilePath))
            {
                var saveData = new Dictionary<string, SaveData>();
                using var fileStream = new FileStream(FilePath, FileMode.Open, FileAccess.Read);
                while (fileStream.Position < fileStream.Length)
                {
                    byte[] systemNameLengthBytes = new byte[sizeof(int)];
                    fileStream.Read(systemNameLengthBytes, 0, systemNameLengthBytes.Length);
                    int systemNameLength = BitConverter.ToInt32(systemNameLengthBytes, 0);

                    byte[] systemNameBytes = new byte[systemNameLength];
                    fileStream.Read(systemNameBytes, 0, systemNameBytes.Length);
                    string systemName = System.Text.Encoding.UTF8.GetString(systemNameBytes);

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
                XiLogger.Log($"File not exists. FilePath = {FilePath}");
                CachedSaveData = null;
            }
        }

        private void OnDestroy()
        {
            try
            {
                OnDestroyAction?.Invoke();
            }
            catch (Exception e)
            {
                XiLogger.LogException(e);
            }

            OnDestroyAction = null;
        }
    }
}
