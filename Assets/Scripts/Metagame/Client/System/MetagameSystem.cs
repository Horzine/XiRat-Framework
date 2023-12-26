using System;
using System.Collections.Generic;
using MemoryPack;

namespace Xi.Metagame.Client.System
{
    public interface IMetagameSystemObserver { }
    public abstract class MetagameSystem
    {
        protected List<IMetagameSystemObserver> _observers = new();
        public readonly string systemName;
        public MetagameSystem(string systemName) => this.systemName = systemName;
        public abstract byte[] ClaimSaveData();
        public abstract void OnSetupAsSystemDefault();
        public abstract void OnUpdateSystem(byte[] data);
        public void AddObserver(IMetagameSystemObserver observer) => _observers.Add(observer);
        public void RemoveObserver(IMetagameSystemObserver observer) => _observers.Remove(observer);
        protected virtual void NotifyObserver(Action<IMetagameSystemObserver> action)
        {
            if (action == null)
            {
                return;
            }

            for (int i = _observers.Count - 1; i >= 0; i--)
            {
                var observer = _observers[i];
                action.Invoke(observer);
            }
        }
    }
    public abstract class MetagameSystem<TSystemData, TObserver> : MetagameSystem
        where TSystemData : MetagameSystemData, new()
        where TObserver : IMetagameSystemObserver
    {
        protected MetagameSystem(string systemName) : base(systemName) { }

        protected TSystemData Data { get; private set; }

        public sealed override byte[] ClaimSaveData()
        {
            Data.PreSerializeData();
            return MemoryPackSerializer.Serialize(Data);
        }

        public sealed override void OnSetupAsSystemDefault()
        {
            Data = new TSystemData();
            Data.OnCreateAsDefaultData();
            Data.PostDeserializeData();
        }

        public sealed override void OnUpdateSystem(byte[] data)
        {
            Data = MemoryPackSerializer.Deserialize<TSystemData>(data);
            Data.PostDeserializeData();
        }

        protected override void NotifyObserver(Action<IMetagameSystemObserver> action)
        {
            if (action == null)
            {
                return;
            }

            NotifyObserver(observer => action.Invoke(observer));
        }

        protected void NotifyObserver(Action<TObserver> action)
        {
            if (action == null)
            {
                return;
            }

            for (int i = _observers.Count - 1; i >= 0; i--)
            {
                var observer = (TObserver)_observers[i];
                action.Invoke(observer);
            }
        }
    }
}
