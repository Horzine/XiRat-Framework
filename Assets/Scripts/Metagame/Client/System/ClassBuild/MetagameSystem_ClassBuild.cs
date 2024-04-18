namespace Xi.Metagame.Client.System.ClassBuild
{
    public class MetagameSystem_ClassBuild : MetagameSystem<ClassBuildData, ISystemObserver_ClassBuild>
    {
        public MetagameSystem_ClassBuild() : base(MetagameSystemNameConst.kClassBuild)
        {

        }
    }

    public interface ISystemObserver_ClassBuild : IMetagameSystemObserver
    {
    }
}

