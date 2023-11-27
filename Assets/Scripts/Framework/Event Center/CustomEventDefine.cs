using System;

namespace Xi.Framework
{
    [AttributeUsage(AttributeTargets.Class)]
    public class CustomEventAttribute : Attribute
    {
        public CustomEventDefine.EventId EventId { get; }
        public CustomEventAttribute(CustomEventDefine.EventId eventId) => EventId = eventId;
    }
}

namespace Xi.Framework
{
    public static class CustomEventDefine
    {
        public enum EventId
        {
            Event1,
            Event2,
            Event3
            // 添加更多的事件
        }

        [CustomEvent(EventId.Event1)]
        public class Event1Event : CustomEvent
        {
            // 自定义事件 Event1 的参数
        }

        [CustomEvent(EventId.Event2)]
        public class Event2Event : CustomEvent
        {
            // 自定义事件 Event2 的参数
        }
    }
}