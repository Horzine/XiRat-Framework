using UnityEngine;
using Xi.Tools;

namespace Xi.TestCase
{
    public class Test_Logger : MonoBehaviour
    {
        private void Update() => XiLogger.LogError(Time.time.ToString(), this);
    }
}
