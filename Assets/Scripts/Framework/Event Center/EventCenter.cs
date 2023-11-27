using System;
using System.Collections.Generic;
using System.Reflection;
using Cysharp.Threading.Tasks;
using Xi.Tools;

namespace Xi.Framework
{
    public abstract class CustomEvent { }
    public interface IEventListener { }
    public interface IEventListener<T> : IEventListener where T : CustomEvent
    {
        void OnEventFire(T customEvent);
    }
    public class EventCenter : MonoSingleton<EventCenter>, ISingleton
    {
        private readonly Dictionary<Type, CustomEventDefine.EventId> _eventMapping = new();
        private readonly Dictionary<int, List<IEventListener>> _allEvent = new();
        private readonly List<Action> pendingOperations = new();
        private bool isFiringEvent = false;

        void ISingleton.OnCreate()
        {

        }

        public async UniTask InitAsync(IReadOnlyCollection<Type> allTypeInAssembly)
        {
            foreach (var type in allTypeInAssembly)
            {
                if (typeof(CustomEvent).IsAssignableFrom(type))
                {
                    var customEvent = type.GetCustomAttribute<CustomEventAttribute>();
                    if (customEvent != null)
                    {
                        var eventId = customEvent.EventId;
                        if (_eventMapping.ContainsKey(type))
                        {
                            throw new ArgumentException($"Type '{type.Name}' is already mapped to EventId '{_eventMapping[type]}'");
                        }

                        _eventMapping[type] = eventId;
                        _allEvent[(int)eventId] = new List<IEventListener>();
                    }
                }
            }

            await UniTask.Yield();
        }

        public void AddListener<T>(IEventListener<T> listener) where T : CustomEvent
        {
            if (isFiringEvent)
            {
                pendingOperations.Add(() => AddListener(listener));
                return;
            }

            var eventType = typeof(T);
            if (!_eventMapping.ContainsKey(eventType))
            {
                throw new ArgumentException($"EventType '{eventType.Name}' is not mapped to any EventId");
            }

            var eventId = _eventMapping[eventType];
            var listeners = _allEvent[(int)eventId];

            if (!listeners.Contains(listener))
            {
                listeners.Add(listener);
            }
            else
            {
                XiLogger.LogError($"Listener {listener} not found for event {eventId}");
            }
        }

        public void RemoveListener<T>(IEventListener<T> listener) where T : CustomEvent
        {
            var eventType = typeof(T);
            if (isFiringEvent)
            {
                pendingOperations.Add(() => RemoveListener(listener));
                return;
            }

            if (!_eventMapping.ContainsKey(eventType))
            {
                throw new ArgumentException($"EventType '{eventType.Name}' is not mapped to any EventId");
            }

            var eventId = _eventMapping[eventType];
            var listeners = _allEvent[(int)eventId];

            if (listeners.Contains(listener))
            {
                listeners.Remove(listener);
            }
            else
            {
                XiLogger.LogError($"Listener {listener} not found for event {eventId}");
            }
        }

        public void FireEvent<T>(T customEvent) where T : CustomEvent
        {
            var eventType = typeof(T);
            if (isFiringEvent)
            {
                throw new InvalidOperationException("Cannot call FireEvent while already firing an event");
            }

            if (!_eventMapping.ContainsKey(eventType))
            {
                throw new ArgumentException($"EventType '{eventType.Name}' is not mapped to any EventId");
            }

            var eventId = _eventMapping[eventType];
            var listeners = _allEvent[(int)eventId];
            isFiringEvent = true;
            foreach (var listener in listeners)
            {
                if (listener is IEventListener<T> eventListener)
                {
                    eventListener.OnEventFire(customEvent);
                }
            }

            isFiringEvent = false;
            foreach (var pendingOperation in pendingOperations)
            {
                pendingOperation.Invoke();
            }

            pendingOperations.Clear();
        }
    }
}