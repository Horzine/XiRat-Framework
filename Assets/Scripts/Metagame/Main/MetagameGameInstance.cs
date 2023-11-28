using Xi.Framework;
using Xi.Tools;

namespace Xi.Metagame
{
    public class MetagameGameInstance : GameInstance
    {
        public override string SceneName { get => SceneNameConst.kMainScene; set { } }
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

    public static class MetagameGameInstance_Extend
    {
        public static MetagameGameInstance CreateMetagameGameInstance() => new();
    }
}
