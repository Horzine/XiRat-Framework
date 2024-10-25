using Unity.Burst;
using Unity.Collections;
using UnityEngine;
using Xi.Framework;
using Xi_;

namespace Xi.TestCase
{
    public class JobBatchManagerTest : MonoBehaviour
    {
        private CalculationJob job;
        public void Start()
        {
            job  = new CalculationJob();
            MyJobBatchManager.Instance.AddJob(job);
        }
    }
}