using System.Collections.Generic;
using UnityEngine;
using Xi.Extend;

namespace Xi.TestCase
{
    public class Test_Collection : MonoBehaviour
    {
        private void Start() =>
            //Func_1();
            Func_2();

        private void Func_1()
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

        private void Func_2()
        {
            var fList = new ForeachMutableList<int>() { 1, 2, 3, 4, 5, 6, 7, 8, };
            foreach (int item in fList)
            {
                if (item == 1)
                {
                    fList.Add(100);
                }

                if (item == 4)
                {
                    fList.Remove(2);
                }

                if (item == 100)
                {
                    fList.Add(1000);
                }
            }
            foreach (var item in fList)
            {
                Debug.Log(item);
            }
            foreach (int item in fList)
            {
                if (item == 1)
                {
                    fList.Add(100);
                }

                if (item == 4)
                {
                    fList.Remove(2);
                }

                if (item == 100)
                {
                    fList.Add(1000);
                }
            }

            foreach (var item in fList)
            {
                Debug.Log(item);
            }
        }
    }
}
