using MemoryPack;

namespace Xi.Metagame.Client.System
{
    public abstract class MetagameSystem
    {
        public readonly string systemName;
        public MetagameSystem(string systemName) => this.systemName = systemName;
        public abstract byte[] ClaimSaveData();
        public abstract void OnSeupAsSystemDefault();
        public abstract void OnUpdateSystem(byte[] data);
    }
    public abstract class MetagameSystem<TSystemData> : MetagameSystem
        where TSystemData : MetagameSystemData, new()
    {
        protected MetagameSystem(string systemName) : base(systemName) { }

        protected TSystemData Data { get; private set; }

        public sealed override byte[] ClaimSaveData()
        {
            Data.PreSerializeData();
            return MemoryPackSerializer.Serialize(Data);
        }

        public sealed override void OnSeupAsSystemDefault()
        {
            Data = new TSystemData();
            Data.OnCreateAsDefaultData();
        }

        public sealed override void OnUpdateSystem(byte[] data)
        {
            Data = MemoryPackSerializer.Deserialize<TSystemData>(data);
            Data.PostDeserializeData();
        }
    }
}
