using UnityEngine;
using Xi.Tools;

namespace Xi.TestCase
{
    public class Test_Logger : MonoBehaviour
    {
        //private void Update() => XiLogger.LogError(Time.time.ToString(), this);

        private void Start()
        {
            Test_Exception();
        }

        private void Test_Exception()
        {
            try
            {
                var ri = GetComponent<Rigidbody>();
                ri = null;
                ri.useGravity = true;
            }
            catch (System.Exception e)
            {
                XiLogger.LogException(e);
            }
        }
    }
}
