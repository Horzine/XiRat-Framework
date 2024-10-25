using System.Collections;
using System.Collections.Generic;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;
using Xi.Framework;

namespace Xi_
{
    [BurstCompile]
    public struct CalculationJob : IBatchJob<float>
    {
        public int index;
        public NativeArray<float> result;

        public int DataCount => 200;

        public void Execute()
        {
            for (int i = 0; i < result.Length; i++)
            {
                result[i] = math.sqrt(index + i);
            }
        }

        public void ExecuteJob(ref float data)
        {
            for (int i = 0; i < result.Length; i++)
            {
                data = math.sqrt(index + i);
            }
        }
    }
    public class MyJobBatchManager : JobBatchManager<CalculationJob, float>
    {
        protected override NativeSlice<float> GetJobDataSlice(CalculationJob job)
        {

        }

        protected override void SetJobResultBatch(ref CalculationJob job, NativeSlice<float> resultSlice)
        {

        }
    }
}
