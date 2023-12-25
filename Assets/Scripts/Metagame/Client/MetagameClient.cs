using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Xi.Framework;
using Xi.Metagame.Client.System;
using Xi.Metagame.Client.System.ClassBuild;
using Xi.Metagame.Client.System.User;
using Xi.Tools;
using static Xi.Framework.UserArchiveManager;

namespace Xi.Metagame.Client
{
    public class MetagameClient
    {
        private readonly Dictionary<string, MetagameSystem> _allSystem = new();

        public MetagameClient()
        {
            this.CreateAllSystemInstance();

            var saveData = UserArchiveManager.Instance.CachedSaveData;
            if (saveData == null)
            {
                XiLogger.Log("SaveData is null");
                saveData = new Dictionary<string, SaveData>();
                foreach (var item in _allSystem.Values)
                {
                    item.OnSetupAsSystemDefault();
                }

                SaveAllSystemData();
                return;
            }

            foreach (var system in _allSystem.Values)
            {
                if (saveData.TryGetValue(system.systemName, out var systemData))
                {
                    system.OnUpdateSystem(systemData.data);
                }
                else
                {
                    XiLogger.Log($"SaveData not contains '{system.systemName}', set as default");
                    system.OnSetupAsSystemDefault();
                }
            }

            UserArchiveManager.Instance.OnDestroyAction += OnUserArchiveDestroyCallback;
        }

        private void OnUserArchiveDestroyCallback() => SaveAllSystemData(false);

        public void SaveAllSystemData(bool isAsync = true)
        {
            var allData = new Dictionary<string, SaveData>();

            foreach (var system in _allSystem.Values)
            {
                byte[] systemData = system.ClaimSaveData();
                allData.Add(system.systemName, new SaveData(system.systemName, systemData));
            }

            if (isAsync)
            {
                UserArchiveManager.Instance.SaveAsync(allData).Forget();
            }
            else
            {
                UserArchiveManager.Instance.SaveSync(allData);
            }
        }

        public void AddSystem(MetagameSystem system) => _allSystem.Add(system.systemName, system);

        public T GetSystem<T>(string systemName) where T : MetagameSystem => _allSystem.TryGetValue(systemName, out var system) ? system as T : null;
    }

    public static class MetagameClient_Extend
    {
        public static void CreateAllSystemInstance(this MetagameClient metagameClient)
        {
            metagameClient.AddSystem(new MetagameSystem_User(MetagameSystemNameConst.kUser));
            metagameClient.AddSystem(new MetagameSystem_ClassBuild(MetagameSystemNameConst.kClassBuild));
        }
    }
}
