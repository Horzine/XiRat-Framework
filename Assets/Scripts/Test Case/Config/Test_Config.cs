using UnityEngine;
using Xi.Config;

namespace Xi.TestCase
{
    public class Test_Config : MonoBehaviour
    {
        private void Start()
        {
            var collection = new ConfigCollection();
            collection.Init();

            foreach (var item in collection.AllTemplate.Values)
            {
                Debug.Log($"{item.Key}\t{item.Type}\t{item.Value}\t{string.Join(',', item.Description)}");
            }


            foreach (var item in collection.AllUnit.Values)
            {
                Debug.Log(item.J["test"]);
            }
        }
    }
}
