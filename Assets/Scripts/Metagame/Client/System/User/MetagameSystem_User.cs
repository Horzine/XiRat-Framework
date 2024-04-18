namespace Xi.Metagame.Client.System.User
{
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

