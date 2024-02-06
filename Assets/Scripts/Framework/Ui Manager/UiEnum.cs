namespace Xi.Framework
{
    public enum UiEnum
    {
        None,
        ________Default_Above____,// This Above SortOrder = kDefaultOrder(0), No sorting
        //=================================================================================
        ________SortOrder_Below____,// This Below will sort by Enum number value

        Metagame_MainMenu,
        ________SortOrder_Default____,// This mean SortOrder is kDefaultOrder(0);
        Metagame_ClassBuild,
        Metagame_SelectMap,
    }

    public static class UiEnum_Extend
    {
        public const int kDefaultOrder = 0;
        public static int GetSortingOrder(UiEnum uiEnum)
        {
            int uiInt = (int)uiEnum;
            int defaultAbove = (int)UiEnum.________Default_Above____;
            int sortOrderBelow = (int)UiEnum.________SortOrder_Below____;
            int sortOrderDefault = (int)UiEnum.________SortOrder_Default____;

            return uiInt < defaultAbove
                ? kDefaultOrder
                : uiInt > sortOrderBelow
                ? uiInt - sortOrderDefault + kDefaultOrder
                : kDefaultOrder;
        }
    }
}
