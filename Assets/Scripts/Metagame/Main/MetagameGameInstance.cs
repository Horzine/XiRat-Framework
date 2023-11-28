using UnityEngine;
using Xi.Framework;
using Xi.Gameplay;
using Xi.Tools;

namespace Xi.Metagame
{
    public class MetagameGameInstance : GameInstance
    {
        public override string SceneName { get => SceneNameConst.kMainScene; protected set { } }
        protected override void OnCreate() => XiLogger.Log(string.Empty);
        protected override GameInstanceObject AddGameInstanceObjectComponent(GameObject go) => go.AddComponent<MetagameGameInstanceObject>();
        protected override void AfterNewSceneActiveAndCreateObject(IGameInstance oldGameInstance, GameInstanceObject gameInstanceObject)
        {
            XiLogger.Log($"oldGameInstance: {oldGameInstance}, gameInstanceObject: {gameInstanceObject}");
            if (oldGameInstance == null)
            {
                return;
            }

            if (oldGameInstance is MetagameGameInstance oldMetagame)
            {
                return;
            }

            if (oldGameInstance is GameplayGameInstance oldGameplay)
            {

            }
        }
        protected override void WillBeReplaced()
        {
            _gameInstanceObject = null;
            XiLogger.Log(string.Empty);
        }
    }

    public static class MetagameGameInstance_Extend
    {
        public static MetagameGameInstance CreateMetagameGameInstance() => new();

        public static MetagameGameInstance GetMetagameGameInstance(this GameMain gameMain)
            => gameMain.CurrentGameInstance is MetagameGameInstance metagameGameInstance ? metagameGameInstance : null;
    }
}
