using MemoryPack;

namespace Xi.Metagame.Client.System.ClassBuild
{
    [MemoryPackable]
    public partial class ClassBuildData : MetagameSystemData
    {
        public override void OnCreateAsDefaultData() { }
        public override void PostDeserializeData() { }
        public override void PreSerializeData() { }
    }
    public class MetagameSystem_ClassBuild : MetagameSystem<ClassBuildData, ISystemObserver_ClassBuild>
    {
        public MetagameSystem_ClassBuild(string systemName) : base(systemName)
        {

        }
    }

    public interface ISystemObserver_ClassBuild : IMetagameSystemObserver
    {
    }
}

