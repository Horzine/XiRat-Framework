using UnityEngine;
using Xi.Tools;

namespace Xi.Framework
{
    public class LocalTestObject : MonoBehaviour
    {
        private void Awake()
        {
            Bootstrap.InitAllManager();
            XiLogger.Log("Init All Manager Finish");
        }
    }
}
