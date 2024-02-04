using System.Collections.Generic;

namespace Xi.Framework
{
    public static partial class CustomEventDefine
    {
        public enum EventId
        {
            Event1Event = 0,
            Event2Event = 1,
            Event2Event4 = 2,
            Event2Event6 = 3,
        }

        public static int nextEventId = 4;

        public static Dictionary<string, EventId> Map { get; } = new()
        {
            { "Xi.Framework.CustomEventDefine+Event1Event", EventId.Event1Event },
            { "Xi.Framework.CustomEventDefine+Event2Event", EventId.Event2Event },
            { "Xi.Framework.CustomEventDefine+Event2Event4", EventId.Event2Event4 },
            { "Xi.Framework.CustomEventDefine+Event2Event6", EventId.Event2Event6 },
        };
    }
}