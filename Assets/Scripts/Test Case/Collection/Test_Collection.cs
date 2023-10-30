using System.Collections.Generic;
using UnityEngine;
using Xi.Framework.Extend;

namespace Xi.TestCase
{
    public class Test_Collection : MonoBehaviour
    {
        private void Start()
        {
            string space = "__________";
            ExtendCollection.Foreach(3, (x) => Debug.Log(x));
            Debug.Log(space);
            ExtendCollection.Foreach(4, 3, (y, x) => Debug.Log($"{x}/{y}"));
            Debug.Log(space);
            var list = new List<int> { 1, 2, 3, 4, 5, };
            list.ForEach((x) => Debug.Log(x));
            Debug.Log(space);
            var dic = new Dictionary<int, int> { { 1, 3 }, { 2, 3 }, { 3, 5 } };
            dic.Foreach((kvPair) => Debug.Log(kvPair));
            Debug.Log(space);
            dic.Foreach(x => Debug.Log(x));
            Debug.Log(space);
            dic.ForeachKey(k => Debug.Log(k));
            Debug.Log(space);
            dic.ForeachValue(v => Debug.Log(v));
            Debug.Log(space);
        }
    }
}
