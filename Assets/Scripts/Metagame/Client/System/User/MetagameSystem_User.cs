using MemoryPack;

namespace Xi.Metagame.Client.System.User
{
    [MemoryPackable]
    public partial class UserData : MetagameSystemData
    {
        public override void OnCreateAsDefaultData() { }
        public override void PostDeserializeData() { }
        public override void PreSerializeData() { }
    }
    public class MetagameSystem_User : MetagameSystem<UserData>
    {
        public MetagameSystem_User(string systemName) : base(systemName)
        {
        }
    }
}

