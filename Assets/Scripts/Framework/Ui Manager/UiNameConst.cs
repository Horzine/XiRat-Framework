namespace Xi.Framework
{
    public static class UiFeatureNameConst
    {
        public const string kMetagame_MainMenu = "MainMenu";
        public const string kMetagame_ClassBuild = "ClassBuild";
        public const string kMetagame_SelectMap = "SelectMap";
    }

    public static class UiPrefabNameConst
    {
        public const string kMetagame_MainMenu = "Ui_Metagame_MainMenu";
        public const string kMetagame_ClassBuild = "Ui_Metagame_ClassBuild";
        public const string kMetagame_SelectMap = "Ui_Metagame_SelectMap";
    }

    public static class UiNameConst_Extend
    {
        public static string AddressableName((string groupName, string uiFeatureName, string uiPrefabName) prefabAssetName)
             => $"{prefabAssetName.groupName}/{prefabAssetName.uiFeatureName}/{prefabAssetName.uiPrefabName}{AssetSuffix.kPrefabSuffix}";
    }
}
