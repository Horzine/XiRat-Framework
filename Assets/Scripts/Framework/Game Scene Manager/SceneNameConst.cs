namespace Xi.Framework
{
    public static class SceneNameConst
    {
        public const string kBootstrap = "Bootstrap";
        public const string kMainScene = "MainScene";
        public const string kTest_Case = "Test_Case";
        public const string kMap_1 = "Map_1";
    }

    public static class SceneNameConst_Extend
    {
        public static string SceneAddressableName(string sceneName) => $"{AssetGroupNameConst.kAddressableGroupName_Scene}/{sceneName}";
    }
}
