using System;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Profiling;

namespace Xi.TestCase
{
    public class JobSystemTest : MonoBehaviour
    {
        private static readonly int _numberOfCalculations = 200;

        private void Update()
        {
            Profiler.BeginSample($"{nameof(JobSystemTest)}");
            PerformJobCalculations();
            Profiler.EndSample();
        }

        private void PerformJobCalculations()
        {
            // 创建一个 NativeArray 来存储结果
            NativeArray<NativeArray<float>> results = new NativeArray<NativeArray<float>>(_numberOfCalculations, Allocator.TempJob);

            JobHandle jobHandle = new JobHandle();

            for (int i = 0; i < _numberOfCalculations; i++)
            {
                // 为每个计算创建一个 NativeArray 来存储该 Job 的结果
                NativeArray<float> result = new NativeArray<float>(_numberOfCalculations, Allocator.TempJob);
                results[i] = result;

                // 创建并调度 Job
                CalculationJob job = new CalculationJob
                {
                    index = i,
                    result = result
                };

                jobHandle = job.Schedule(jobHandle);

                jobHandle.Complete();
            }

            // 释放 NativeArray 资源
            for (int i = 0; i < _numberOfCalculations; i++)
            {
                results[i].Dispose();
            }
            results.Dispose();
        }

        [BurstCompile]
        public struct CalculationJob : IJob
        {
            public int index;
            public NativeArray<float> result;

            public void Execute()
            {
                for (int i = 0; i < result.Length; i++)
                {
                    result[i] = math.sqrt(index + i);
                }
            }
        }
    }
}
