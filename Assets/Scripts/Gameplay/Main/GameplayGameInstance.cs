using Xi.Framework;
using Xi.Tools;

namespace Xi.Gameplay
{
    public class GameplayGameInstance : GameInstance
    {
        public override string SceneName { get; set; }
        public override void OnCreate() => XiLogger.Log("");
        public override void OnSceneActive(IGameInstance oldGameInstance)
        {
            if (oldGameInstance == null)
            {
                return;
            }

            XiLogger.Log($"oldGameInstance: {oldGameInstance}");
        }
    }

    public static class GameplayGameInstance_Extend
    {
        public static GameplayGameInstance CreateGameplayGameInstance() => new();
    }
}
