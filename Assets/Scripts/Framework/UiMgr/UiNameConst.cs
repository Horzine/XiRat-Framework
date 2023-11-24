namespace Xi.Framework
{
    public static class UiFeatureNameConst
    {
        public const string kSystem_A = "System A";
        public const string kSystem_B = "System B";
        public const string kSystem_C = "System C";
        public const string kSystem_D = "System D";
    }

    public static class UiPrefabNameConst
    {
        public const string kSystem_A_name = "UiWindow_SystemA";
        public const string kSystem_B_name = "UiWindow_SystemB";
        public const string kSystem_C_name = "UiWindow_SystemC";
        public const string kSystem_D_name = "UiWindow_SystemD";
    }

    public static class UiNameConst_Extend
    {
        public static string AddressableName((string groupName, string uiFeatureName, string uiPrefabName) prefabAssetName)
             => $"{prefabAssetName.groupName}/{prefabAssetName.uiFeatureName}/{prefabAssetName.uiPrefabName}{AssetSuffix.kPrefabSuffix}";
    }
}
