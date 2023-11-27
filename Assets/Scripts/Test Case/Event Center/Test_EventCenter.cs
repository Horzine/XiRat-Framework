using UnityEngine;
using Xi.Framework;
using static Xi.Framework.CustomEventDefine;

namespace Xi.TestCase
{
    public class MyEventListener : IEventListener<Event1Event>, IEventListener<Event2Event>
    {
        public void Start()
        {
            EventCenter.Instance.AddListener<Event1Event>(this);
            EventCenter.Instance.AddListener<Event2Event>(this);
        }

        public void Stop()
        {
            EventCenter.Instance.RemoveListener<Event1Event>(this);
            EventCenter.Instance.RemoveListener<Event2Event>(this);
        }

        void IEventListener<Event1Event>.OnEventFire(Event1Event eventArgs)
        {
            Debug.Log($"{GetHashCode()}: Event1 EventArgs received");
            EventCenter.Instance.AddListener<Event1Event>(new MyEventListener());
        }

        void IEventListener<Event2Event>.OnEventFire(Event2Event eventArgs) => Debug.Log($"{GetHashCode()}: Event2 EventArgs received");
    }

    public class Test_EventCenter : MonoBehaviour
    {
        private async void Start()
        {
            await EventCenter.Instance.InitAsync(Bootstrap.GetTypesFromAssembly());

            var obj = new MyEventListener();
            obj.Start();

            EventCenter.Instance.FireEvent(new Event1Event());
            EventCenter.Instance.FireEvent(new Event2Event());

            obj.Stop();
            EventCenter.Instance.FireEvent(new Event1Event());
        }
    }
}