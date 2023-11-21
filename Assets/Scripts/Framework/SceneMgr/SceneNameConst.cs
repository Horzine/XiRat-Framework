namespace Xi.Framework
{
    public static class SceneNameConst
    {
        public const string kAddressableGroupName = "SceneInBuild";
        public const string kBoost = "Boost";
        public const string kMain = "Main";
        public const string kTest_Case = "Test_Case";
        public const string kMap_1 = "Map_1";

        public static string AddressableName(string sceneName) => $"{kAddressableGroupName}/{sceneName}";
    }
}
