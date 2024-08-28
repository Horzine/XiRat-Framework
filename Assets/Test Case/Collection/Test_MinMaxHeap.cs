using UnityEngine;
using Xi.Extension.Collection;

namespace Xi.TestCase
{
    public class Test_MinMaxHeap : MonoBehaviour
    {
        private void Start()
        {
            var minHeap = new MinHeap<int>
            {
                2,
                3,
                4,
                1,
                1,
                5,
                -1,
            };

            Debug.Log(minHeap.ExtractMin());
            Debug.Log(minHeap.ExtractMin());
            Debug.Log(minHeap.ExtractMin());
            Debug.Log(minHeap.ExtractMin());
            Debug.Log(minHeap.ExtractMin());
            Debug.Log(minHeap.ExtractMin());
            Debug.Log(minHeap.ExtractMin());

            var heap2 = new MinHeap<string, int>
            {
                { "item1", 3 },
                { "item2", 4 },
                { "item3", 5 }
            };
            var curMin = heap2.Min;
            Debug.Log($"{curMin.Key}, {curMin.Value}");
            heap2.ChangeValue("item2", 1);   // Now value of "item2" is 1.
            var item = heap2.ExtractMin();      // Returns 1.
            Debug.Log($"{item.Key}, {item.Value}");
        }
    }
}
