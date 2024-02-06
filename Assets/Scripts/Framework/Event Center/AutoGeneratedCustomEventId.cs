using System.Collections.Generic;

namespace Xi.Framework
{
    public static partial class CustomEventDefine
    {
        public static int nextEventId = 11;

        public static Dictionary<string, int> TypeNameMapInt { get; } = new()
        {
            { "Xi.Framework.CustomEventDefine+Event1Event", 0 },
            { "Xi.Framework.CustomEventDefine+Event2Event", 1 },
            { "Xi.Framework.CustomEventDefine+Event2Event4", 2 },
            { "Xi.Framework.CustomEventDefine+Event2Event12", 5 },
            { "Xi.Framework.CustomEventDefine+Event2Event13", 6 },
            { "Xi.Framework.CustomEventDefine+Event2Event14", 7 },
            { "Xi.Framework.CustomEventDefine+Event2Event16", 8 },
            { "Xi.Framework.CustomEventDefine+Event2Event17", 9 },
            { "Xi.Framework.CustomEventDefine+Event2Event19", 10 },
        };

        public static Dictionary<int, string> IntMapTypeName { get; } = new()
        {
            { 0, "Xi.Framework.CustomEventDefine+Event1Event" },
            { 1, "Xi.Framework.CustomEventDefine+Event2Event" },
            { 2, "Xi.Framework.CustomEventDefine+Event2Event4" },
            { 5, "Xi.Framework.CustomEventDefine+Event2Event12" },
            { 6, "Xi.Framework.CustomEventDefine+Event2Event13" },
            { 7, "Xi.Framework.CustomEventDefine+Event2Event14" },
            { 8, "Xi.Framework.CustomEventDefine+Event2Event16" },
            { 9, "Xi.Framework.CustomEventDefine+Event2Event17" },
            { 10, "Xi.Framework.CustomEventDefine+Event2Event19" },
        };

        public static Dictionary<int, int[]> InheritanceChainMap { get; } = new()
        {
            { 0, new int[] {  } },
            { 1, new int[] {  } },
            { 2, new int[] {  } },
            { 5, new int[] { 2 } },
            { 6, new int[] { 5, 2 } },
            { 7, new int[] { 6, 5, 2 } },
            { 8, new int[] { 6, 5, 2 } },
            { 9, new int[] { 8, 6, 5, 2 } },
            { 10, new int[] {  } },
        };
    }
}