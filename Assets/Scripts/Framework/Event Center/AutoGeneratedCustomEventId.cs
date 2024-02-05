using System.Collections.Generic;

namespace Xi.Framework
{
    public static partial class CustomEventDefine
    {
        public enum EventId
        {
            Event1Event = 0,
            Event2Event = 1,
            Event2Event4 = 7,
            Event2Event10 = 5,
            Event2Event11 = 6,
            Event2Event12 = 8,
            Event2Event13 = 9,
            Event2Event14 = 10,
            Event2Event16 = 11,
            Event2Event17 = 12,
        }

        public static int nextEventId = 13;

        public static Dictionary<string, EventId> TypeNameMapEventId { get; } = new()
        {
            { "Xi.Framework.CustomEventDefine+Event1Event", EventId.Event1Event },
            { "Xi.Framework.CustomEventDefine+Event2Event", EventId.Event2Event },
            { "Xi.Framework.CustomEventDefine+Event2Event4", EventId.Event2Event4 },
            { "Xi.Framework.CustomEventDefine+Event2Event10", EventId.Event2Event10 },
            { "Xi.Framework.CustomEventDefine+Event2Event11", EventId.Event2Event11 },
            { "Xi.Framework.CustomEventDefine+Event2Event12", EventId.Event2Event12 },
            { "Xi.Framework.CustomEventDefine+Event2Event13", EventId.Event2Event13 },
            { "Xi.Framework.CustomEventDefine+Event2Event14", EventId.Event2Event14 },
            { "Xi.Framework.CustomEventDefine+Event2Event16", EventId.Event2Event16 },
            { "Xi.Framework.CustomEventDefine+Event2Event17", EventId.Event2Event17 },
        };

        public static Dictionary<int, int[]> InheritanceChainMap { get; } = new()
        {
            { 0, new int[] {  } },
            { 1, new int[] {  } },
            { 5, new int[] {  } },
            { 6, new int[] {  } },
            { 7, new int[] {  } },
            { 8, new int[] { 6 } },
            { 9, new int[] { 6 } },
            { 10, new int[] { 9, 6 } },
            { 11, new int[] { 5 } },
            { 12, new int[] { 11, 5 } },
        };
    }
}