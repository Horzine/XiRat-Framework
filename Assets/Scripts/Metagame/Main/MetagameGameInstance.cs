using UnityEngine;
using Xi.Framework;
using Xi.Gameplay.Main;
using Xi.Metagame.Client;
using Xi.Metagame.Scene;
using Xi.Tools;

namespace Xi.Metagame.Main
{
    public class MetagameGameInstance : GameInstance
    {
        public MetagameClient Client { get; private set; }
        public MetagameSceneObjRefHolder SceneObjRefHolder { get; private set; }
        public override string SceneName { get => SceneNameConst.kMainScene; protected set { } }
        protected override void OnCreate() => XiLogger.Log(string.Empty);
        protected override GameInstanceObject AddGameInstanceObjectComponent(GameObject go) => go.AddComponent<MetagameGameInstanceObject>();
        protected override void AfterNewSceneActiveAndCreateObject(IGameInstance oldGameInstance, GameInstanceObject gameInstanceObject)
        {
            XiLogger.Log($"oldGameInstance: {oldGameInstance}, gameInstanceObject: {gameInstanceObject}");
            if (oldGameInstance == null)
            {
            }

            if (oldGameInstance is MetagameGameInstance oldMetagame)
            {
            }

            if (oldGameInstance is GameplayGameInstance oldGameplay)
            {

            }

            Client = new MetagameClient();
            var refHolderObj = Object.FindObjectOfType<SceneObjectReferenceHolderGameObject>();
            if (refHolderObj != null && refHolderObj.TryGetComponent<MetagameSceneObjRefHolder>(out var refHolder))
            {
                SceneObjRefHolder = refHolder;
            }
            else if (refHolderObj == null)
            {
                XiLogger.LogError($"'{SceneName}' scene no contain '{typeof(SceneObjectReferenceHolderGameObject)}' mono behaivour");
            }
            else if (SceneObjRefHolder == null)
            {
                XiLogger.LogError($"'{refHolderObj.name}' no contain '{typeof(MetagameSceneObjRefHolder)}' mono behaivour");
            }
        }
        protected override void WillBeReplaced()
        {
            _gameInstanceObject = null;
            XiLogger.Log(string.Empty);
        }
    }

    public static class MetagameGameInstance_Extension
    {
        public static MetagameGameInstance CreateMetagameGameInstance() => new();

        public static MetagameGameInstance GetMetagameInstance(this GameMain gameMain)
            => gameMain.CurrentGameInstance is MetagameGameInstance metagameGameInstance ? metagameGameInstance : null;

        public static MetagameClient GetMetagameClient(this GameMain gameMain)
            => GetMetagameInstance(gameMain)?.Client;

        public static MetagameSceneObjRefHolder GetMetagameSceneObjRefHolder(this GameMain gameMain)
            => GetMetagameInstance(gameMain)?.SceneObjRefHolder;
    }
}
