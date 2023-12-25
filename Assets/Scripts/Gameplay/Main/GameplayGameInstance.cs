using UnityEngine;
using Xi.Framework;
using Xi.Gameplay.Client;
using Xi.Gameplay.Scene;
using Xi.Metagame.Main;
using Xi.Tools;

namespace Xi.Gameplay.Main
{
    public class GameplayGameInstance : GameInstance
    {
        public GameplayClient Client { get; private set; }
        public GameplaySceneObjRefHolder SceneObjRefHolder { get; private set; }
        public override string SceneName { get; protected set; }
        protected override void OnCreate() => XiLogger.Log(string.Empty);
        protected override GameInstanceObject AddGameInstanceObjectComponent(GameObject go) => go.AddComponent<GameplayGameInstanceObject>();
        protected override void AfterNewSceneActiveAndCreateObject(IGameInstance oldGameInstance, GameInstanceObject gameInstanceObject)
        {
            XiLogger.Log($"oldGameInstance: {oldGameInstance}, gameInstanceObject: {gameInstanceObject}");
            if (oldGameInstance == null)
            {
            }

            if (oldGameInstance is GameplayGameInstance oldGameplay)
            {
            }

            if (oldGameInstance is MetagameGameInstance oldMetagame)
            {
            }

            Client = new GameplayClient();
            SceneObjRefHolder = Object.FindObjectOfType<SceneObjectReferenceHolderGameObject>().GetComponent<GameplaySceneObjRefHolder>();
        }
        protected override void WillBeReplaced()
        {
            _gameInstanceObject = null;
            XiLogger.Log(string.Empty);
        }
    }

    public static class GameplayGameInstance_Extend
    {
        public static GameplayGameInstance CreateGameplayGameInstance() => new();

        public static GameplayGameInstance GetGameplayInstance(this GameMain gameMain)
            => gameMain.CurrentGameInstance is GameplayGameInstance gameplayGameInstance ? gameplayGameInstance : null;

        public static GameplayClient GetGameplayClient(this GameMain gameMain)
            => GetGameplayInstance(gameMain)?.Client;

        public static GameplaySceneObjRefHolder GetGameplaySceneObjRefHolder(this GameMain gameMain)
            => GetGameplayInstance(gameMain)?.SceneObjRefHolder;
    }
}
