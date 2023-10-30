using UnityEngine;

namespace Xi.TestCase
{
    public class Test_Singleton_Boost : MonoBehaviour
    {
        // Start is called before the first frame update
        private void Start()
        {
            var ts = Test_Singleton.Instance;
        }
    }
}
