using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Cysharp.Threading.Tasks;
using Xi.Tools;

namespace Xi.Framework
{
    public abstract class CustomEvent { }
    public interface IEventListener { }
    public interface IEventListener<in T> : IEventListener where T : CustomEvent
    {
        internal void OnEventFire(T customEvent);
    }
    public class EventCenter : MonoSingleton<EventCenter>, ISingleton
    {
        private readonly Dictionary<string, int> _eventMapping = CustomEventDefine.TypeNameMapInt;
        private Dictionary<int, List<IEventListener>> _allEvent;
        private readonly List<Action> _pendingOperations = new();
        private bool _isFiringEvent = false;

        void ISingleton.OnCreate()
        {

        }

        public async UniTask InitAsync()
        {
            Init();
            await UniTask.Yield();
        }

        public void Init()
            => _allEvent = _eventMapping.ToDictionary((item) => item.Value, (itme) => new List<IEventListener>());

        public void AddListener<T>(IEventListener<T> listener) where T : CustomEvent
        {
            if (_isFiringEvent)
            {
                _pendingOperations.Add(() => AddListener(listener));
                return;
            }

            var eventType = typeof(T);
            string fullName = eventType.FullName;
            if (!_eventMapping.TryGetValue(fullName, out int eventId))
            {
                throw new ArgumentException($"EventType '{fullName}' is not mapped to any EventId, Generate event id again");
            }

            var listeners = _allEvent[eventId];

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
            string fullName = eventType.FullName;
            if (!_eventMapping.TryGetValue(fullName, out int eventId))
            {
                throw new ArgumentException($"EventType '{fullName}' is not mapped to any EventId, Generate event id again");
            }

            var listeners = _allEvent[eventId];

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
            if (_isFiringEvent)
            {
                throw new InvalidOperationException("Cannot call FireEvent while already firing an event");
            }

            var eventType = typeof(T);
            string fullName = eventType.FullName;
            if (!_eventMapping.TryGetValue(fullName, out int eventId))
            {
                throw new ArgumentException($"EventType '{fullName}' is not mapped to any EventId, Generate event id again");
            }

            var listeners = _allEvent[eventId];
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

            int[] inheritanceChain = CustomEventDefine.InheritanceChainMap[eventId];
            foreach (int item in inheritanceChain)
            {
                var parentListeners = _allEvent[item];
                foreach (var listener in parentListeners)
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
            }

            _isFiringEvent = false;
            foreach (var pendingOperation in _pendingOperations)
            {
                pendingOperation.Invoke();
            }

            _pendingOperations.Clear();
        }
    }

    public static class EventCenter_Extension
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