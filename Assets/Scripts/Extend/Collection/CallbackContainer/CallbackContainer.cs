using System;
using Xi.Tools;

namespace Xi.Extend.Collection
{
    public abstract class CallbackContainer
    {
        public interface ICallbackEntry { }
    }

    public class CallbackContainer<T> : CallbackContainer where T : CallbackContainer.ICallbackEntry
    {
        private readonly IndexedSet<T> _set = new();

        public void AddCallback(T callbackEntry) => _set.Add(callbackEntry);

        public void RemoveCallback(T callbackEntry) => _set.Remove(callbackEntry);

        public void InvokeCallback(Action<T> action)
        {
            for (int i = _set.Count - 1; i >= 0; i--)
            {
                try
                {
                    action.Invoke(_set[i]);
                }
                catch (Exception e)
                {
                    XiLogger.LogException(e);
                }
            }
        }
    }
}
