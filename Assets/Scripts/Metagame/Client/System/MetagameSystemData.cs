namespace Xi.Metagame.Client.System
{
    public abstract class MetagameSystemData
    {
        public abstract void OnCreateAsDefaultData();
        public abstract void PostDeserializeData();
        public abstract void PreSerializeData();
    }
}
