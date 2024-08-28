using UnityEngine;
using Xi.Extension.Collection;

namespace Xi.TestCase
{
    public class Test_Deque : MonoBehaviour
    {
        private void Start()
        {
            var deque = new Deque<int>();

            deque.AddToBack(0);
            deque.AddToBack(1);
            deque.AddToBack(2);
            deque.AddToBack(3);
            deque.AddToBack(4);
            deque.AddToFront(-1);
            deque.AddToFront(-2);
            deque.AddToFront(-3);
            deque.AddToFront(-4);
            Debug.Log(deque.RemoveFromBack());
            Debug.Log(deque.RemoveFromFront());
            Debug.Log(deque.RemoveFromBack());
            Debug.Log(deque.RemoveFromFront());
            Debug.Log(deque.RemoveFromBack());
            Debug.Log(deque.RemoveFromFront());
            Debug.Log(deque.RemoveFromBack());
            Debug.Log(deque.RemoveFromFront());
            Debug.Log(deque.RemoveFromBack());
            Debug.Log(deque.RemoveFromFront());
        }
    }
}
