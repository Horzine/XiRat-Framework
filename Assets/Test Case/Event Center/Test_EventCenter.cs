using UnityEngine;
using Xi.Framework;
using static Xi.Framework.CustomEventDefine;

namespace Xi.TestCase
{
    public class Your : IEventListener<Event2Event12>
    {
        void IEventListener<Event2Event12>.OnEventFire(Event2Event12 customEvent) => Debug.Log($"{GetHashCode()}: your Event2Event12 EventArgs received");
    }

    public class MyEventListener :
        IEventListener<Event1Event>,
        IEventListener<Event2Event>,
        IEventListener<Event2Event4>
    {
        public void Start()
        {
            EventCenter.Instance.AddListener<Event1Event>(this);
            this.RegisterListener<Event2Event>();
        }

        public void Stop()
        {
            EventCenter.Instance.RemoveListener<Event1Event>(this);
            this.UnregisterListener<Event2Event>();
        }

        void IEventListener<Event1Event>.OnEventFire(Event1Event eventArgs)
        {
            Debug.Log($"ObjectHash({GetHashCode()}): Event1 EventArgs received");
            EventCenter.Instance.AddListener<Event1Event>(new MyEventListener());
        }

        void IEventListener<Event2Event>.OnEventFire(Event2Event eventArgs) => Debug.Log($"{GetHashCode()}: Event2 EventArgs received");

        void IEventListener<Event2Event4>.OnEventFire(Event2Event4 customEvent) => Debug.Log($"{GetHashCode()}: Event2Event4 EventArgs received");

    }

    public class Test_EventCenter : MonoBehaviour
    {
        private async void Start()
        {
            await EventCenter.Instance.InitAsync();

            Test1();

            Test2();
        }

        private static void Test2()
        {
            var obj2 = new MyEventListener();
            EventCenter.Instance.AddListener<Event2Event4>(obj2);

            // 由于接口支持 in 协变， EventCenter.Instance.AddListener<Event2Event12>(obj2); 
            // 这句手动添加语法上支持， 但是调用的时候会被 Call 两次， 因为 EventCenter 会根据继承链调用一次， 手动注册又调用了一次
            // 所以不要手动注册子类

            var your = new Your();

            EventCenter.Instance.AddListener<Event2Event12>(your);

            EventCenter.Instance.FireEvent(new Event2Event12());
        }

        private static void Test1()
        {
            var obj = new MyEventListener();
            obj.Start();

            EventCenter.Instance.FireEvent(new Event1Event());
            EventCenter.Instance.FireEvent(new Event2Event());

            obj.Stop();
            new Event1Event().FireEvent();

            EventCenter.Instance.AddListener<Event2Event4>(obj);
            EventCenter.Instance.FireEvent(new Event2Event4());
        }
    }
}