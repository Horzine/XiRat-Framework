using UnityEngine;
using XiConfig;

namespace Xi.TestCase
{
    public class Test_Config : MonoBehaviour
    {
        // Start is called before the first frame update
        private void Start()
        {
            var collection = new ConfigCollection();
            collection.Init();

            foreach (var item in collection.AllTemplate.Values)
            {
                Debug.Log($"{item.Key}\t{item.Type}\t{item.Value}\t{string.Join(',', item.Description)}");
            }
        }
    }
}
