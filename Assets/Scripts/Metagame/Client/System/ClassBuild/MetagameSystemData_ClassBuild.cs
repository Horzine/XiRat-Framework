using MemoryPack;
using Xi.Metagame.Client.System;

namespace Xi.Metagame
{
    [MemoryPackable]
    public partial class ClassBuildData : MetagameSystemData
    {
        public override void OnCreateAsDefaultData() { }
        public override void PostDeserializeData() { }
        public override void PreSerializeData() { }
    }
}
