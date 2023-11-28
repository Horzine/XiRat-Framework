using UnityEngine;
using Xi.Framework;
using Xi.Metagame;
using Xi.Tools;

namespace Xi.Gameplay
{
    public class GameplayGameInstance : GameInstance
    {
        public override string SceneName { get; protected set; }
        protected override void OnCreate() => XiLogger.Log(string.Empty);
        protected override GameInstanceObject AddGameInstanceObjectComponent(GameObject go) => go.AddComponent<GameplayGameInstanceObject>();
        protected override void AfterNewSceneActiveAndCreateObject(IGameInstance oldGameInstance, GameInstanceObject gameInstanceObject)
        {
            XiLogger.Log($"oldGameInstance: {oldGameInstance}, gameInstanceObject: {gameInstanceObject}");
            if (oldGameInstance == null)
            {
                return;
            }

            if (oldGameInstance is GameplayGameInstance oldGameplay)
            {
                return;
            }

            if (oldGameInstance is MetagameGameInstance oldMetagame)
            {

            }
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

        public static GameplayGameInstance GetGameplayGameInstance(this GameMain gameMain)
            => gameMain.CurrentGameInstance is GameplayGameInstance gameplayGameInstance ? gameplayGameInstance : null;
    }
}
