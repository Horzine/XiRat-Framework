using UnityEngine;
using Xi.Framework;
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

            }
            else
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
    }
}
