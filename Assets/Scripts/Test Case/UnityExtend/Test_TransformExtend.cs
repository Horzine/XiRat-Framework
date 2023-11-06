using UnityEngine;
using Xi.Extend.UnityExtend;

namespace Xi.TestCase
{
    public class Test_TransformExtend : MonoBehaviour
    {
        private void Start()
        {
            transform.SetIdentity();

            SimpleFind();

            print(string.Join<Transform>(',', transform.GetAllDirectChild()));

            transform.DestroyAllChild();

            transform.AddChildAndSetIdentity(new GameObject("new Child").transform);
        }

        private void SimpleFind()
        {
            print(transform.Find("1"));
            print(transform.Find("2"));
            print(transform.Find("1/2"));
        }
    }
}
