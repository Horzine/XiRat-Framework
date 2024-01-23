using System;
using MemoryPack;
using Xi.Extend.Collection;

namespace Xi.Metagame.Client.System
{
    public interface IMetagameSystemObserver : CallbackContainer.ICallbackEntry { }
    public abstract class MetagameSystem
    {
        public readonly string systemName;
        public MetagameSystem(string systemName) => this.systemName = systemName;
        public abstract byte[] ClaimSaveData();
        public abstract void OnSetupAsSystemDefault();
        public abstract void OnUpdateSystem(byte[] data);
    }
    public abstract class MetagameSystem<TSystemData, TObserver> : MetagameSystem
        where TSystemData : MetagameSystemData, new()
        where TObserver : IMetagameSystemObserver
    {
        private readonly CallbackContainer<TObserver> _observers = new();
        protected TSystemData Data { get; private set; }
        protected MetagameSystem(string systemName) : base(systemName) { }

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

        public void AddObserver(TObserver observer) => _observers.AddCallback(observer);

        public void RemoveObserver(TObserver observer) => _observers.RemoveCallback(observer);

        protected void NotifyObserver(Action<TObserver> action)
        {
            if (action == null)
            {
                return;
            }

            _observers.InvokeCallback(action);
        }
    }
}
