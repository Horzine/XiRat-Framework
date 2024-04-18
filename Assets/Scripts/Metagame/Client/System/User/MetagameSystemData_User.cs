using MemoryPack;
using Xi.Metagame.Client.System;

namespace Xi.Metagame
{
    [MemoryPackable]
    public partial class UserData : MetagameSystemData
    {
        [MemoryPackInclude] private string testStr;
        [MemoryPackInclude] public int TestInt { get; set; }
        public override void OnCreateAsDefaultData() => TestInt = 1000;
        public override void PostDeserializeData() => testStr = TestInt.ToString();
        public override void PreSerializeData() => testStr = TestInt.ToString();
    }
}
