using MemoryPack;

namespace Xi.Metagame.Client.System.User
{
    [MemoryPackable]
    public partial class UserData : MetagameSystemData
    {
        [MemoryPackInclude] private string testStr;
        [MemoryPackInclude] public int TestInt { get; set; }
        public override void OnCreateAsDefaultData()
        {
            TestInt = 1000;
            PostDeserializeData();
        }

        public override void PostDeserializeData() => testStr = TestInt.ToString();
        public override void PreSerializeData() => testStr = TestInt.ToString();
    }
    public class MetagameSystem_User : MetagameSystem<UserData>
    {
        public MetagameSystem_User(string systemName) : base(systemName)
        {
        }

        public void SetupTestInt(int num) => Data.TestInt = num;
        public object GetTestInt() => Data.TestInt;
    }
}

