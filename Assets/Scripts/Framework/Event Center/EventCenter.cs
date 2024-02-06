using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using Cysharp.Threading.Tasks;
using Xi.Tools;

namespace Xi.Framework
{
    public abstract class CustomEvent { }
    public interface IEventListener { }
    public interface IEventListener<T> : IEventListener where T : CustomEvent
    {
        internal void OnEventFire(T customEvent);
    }
    public class EventCenter : MonoSingleton<EventCenter>, ISingleton
    {
        private readonly Dictionary<Type, CustomEventDefine.EventId> _eventMapping = new();
        private readonly Dictionary<int, List<IEventListener>> _allEvent = new();
        private readonly List<Action> _pendingOperations = new();
        private bool _isFiringEvent = false;

        void ISingleton.OnCreate()
        {

        }

        public async UniTask InitAsync(IReadOnlyCollection<Type> allTypeInAssembly)
        {
            Init(allTypeInAssembly);
            await UniTask.Yield();
        }

        public void Init(IReadOnlyCollection<Type> allTypeInAssembly)
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
        }

        public void AddListener<T>(IEventListener<T> listener) where T : CustomEvent
        {
            if (_isFiringEvent)
            {
                _pendingOperations.Add(() => AddListener(listener));
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
            if (_isFiringEvent)
            {
                _pendingOperations.Add(() => RemoveListener(listener));
                return;
            }

            var eventType = typeof(T);
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
            if (_isFiringEvent)
            {
                throw new InvalidOperationException("Cannot call FireEvent while already firing an event");
            }

            if (!_eventMapping.ContainsKey(eventType))
            {
                throw new ArgumentException($"EventType '{eventType.Name}' is not mapped to any EventId");
            }

            var eventId = _eventMapping[eventType];
            var listeners = _allEvent[(int)eventId];
            _isFiringEvent = true;
            foreach (var listener in listeners)
            {
                if (listener is IEventListener<T> eventListener)
                {
                    try
                    {
                        eventListener.OnEventFire(customEvent);
                    }
                    catch (Exception e)
                    {
                        XiLogger.LogException(e);
                    }
                }
            }

            _isFiringEvent = false;
            foreach (var pendingOperation in _pendingOperations)
            {
                pendingOperation.Invoke();
            }

            _pendingOperations.Clear();
        }
    }

    public static class EventCenter_Extend
    {
        public static void RegisterListener<T>(this IEventListener<T> eventListener,
            [CallerFilePath] string filePath = "",
            [CallerMemberName] string methodName = "",
            [CallerLineNumber] int lineNumber = -1) where T : CustomEvent
        {
            if (eventListener == null)
            {
                XiLogger.LogError("EventListener is null");
                return;
            }

            EventCenter.Instance.AddListener(eventListener);
            XiLogger.Log($"Register Event Listener: {typeof(T).Name}", filePath: filePath, methodName: methodName, lineNumber: lineNumber);
        }

        public static void UnregisterListener<T>(this IEventListener<T> eventListener,
            [CallerFilePath] string filePath = "",
            [CallerMemberName] string methodName = "",
            [CallerLineNumber] int lineNumber = -1) where T : CustomEvent
        {
            if (eventListener == null)
            {
                XiLogger.LogError("EventListener is null");
                return;
            }

            EventCenter.Instance.RemoveListener(eventListener);
            XiLogger.Log($"Unregister Event Listener: {typeof(T).Name}", filePath: filePath, methodName: methodName, lineNumber: lineNumber);
        }

        public static void FireEvent<T>(this T customEvent,
            [CallerFilePath] string filePath = "",
            [CallerMemberName] string methodName = "",
            [CallerLineNumber] int lineNumber = -1) where T : CustomEvent
        {
            if (customEvent == null)
            {
                XiLogger.LogError("CustomEvent is null");
                return;
            }

            EventCenter.Instance.FireEvent(customEvent);
            XiLogger.Log($"FireEvent: {typeof(T).Name}", filePath: filePath, methodName: methodName, lineNumber: lineNumber);
        }
    }
}