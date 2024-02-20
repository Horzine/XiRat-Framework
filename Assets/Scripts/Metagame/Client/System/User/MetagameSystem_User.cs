using MemoryPack;

namespace Xi.Metagame.Client.System.User
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
    public class MetagameSystem_User : MetagameSystem<UserData, ISystemObserver_User>
    {
        public MetagameSystem_User() : base(MetagameSystemNameConst.kUser)
        {
        }

        public void SetupTestInt(int num)
        {
            Data.TestInt = num;
            NotifyObserver(observer => observer.TestIntChange(Data.TestInt));
        }

        public int GetTestInt() => Data.TestInt;

    }
    public interface ISystemObserver_User : IMetagameSystemObserver
    {
        internal void TestIntChange(int num);
    }
}

