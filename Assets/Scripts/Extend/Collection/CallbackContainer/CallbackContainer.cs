using System;
using Xi.Tools;
using static Xi.Extend.Collection.CallbackContainer;

namespace Xi.Extend.Collection
{
    [AttributeUsage(AttributeTargets.Interface)]
    public class CallbackAttribute : System.Attribute
    {
    }

    public abstract class CallbackContainer : IDisposable
    {
        public abstract void Dispose();

        public interface ICallbackEntry { }
    }

    public class CallbackContainer<T> : CallbackContainer
        where T : ICallbackEntry
    {
        private readonly IndexedSet<T> _set = new();
        private readonly IndexedSet<T> _tempAdd = new();
        private readonly IndexedSet<T> _tempRemove = new();
        private bool _isInvoking;

        public void AddCallback(T callbackEntry)
        {
            if (_isInvoking)
            {
                _tempAdd.Add(callbackEntry);
            }
            else
            {
                _set.Add(callbackEntry);
            }
        }

        public void RemoveCallback(T callbackEntry)
        {
            if (_isInvoking)
            {
                _tempRemove.Add(callbackEntry);
            }
            else
            {
                _set.Remove(callbackEntry);
            }
        }

        public void InvokeCallback(Action<T> action)
        {
            _isInvoking = true;
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

            _isInvoking = false;
            ApplyPendingChanges();
        }

        private void ApplyPendingChanges()
        {
            foreach (var item in _tempAdd)
            {
                _set.Add(item);
            }

            _tempAdd.Clear();

            foreach (var item in _tempRemove)
            {
                _set.Remove(item);
            }

            _tempRemove.Clear();
        }

        public void Clear()
        {
            _set.Clear();
            _tempAdd.Clear();
            _tempRemove.Clear();
        }

        public override void Dispose() => Clear();
    }
}
