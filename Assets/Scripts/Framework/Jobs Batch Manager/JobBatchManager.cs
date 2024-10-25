using System.Collections.Generic;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;

namespace Xi.Framework
{
    public interface IBatchJob<TData>
    {
        void ExecuteJob(ref TData data); // 执行Job逻辑
        int DataCount { get; }           // 获取Job包含的数据数量
    }

    public abstract class JobBatchManager<TJob, TData> : MonoSingleton<JobBatchManager<TJob, TData>>, ISingleton
     where TJob : IBatchJob<TData>
     where TData : unmanaged
    {
        protected NativeArray<TData> batchDataArray; // 批处理的数据数组
        protected List<TJob> jobs = new(); // 存储所有相同类型的Job

        void ISingleton.OnCreate()
        {
            // 在派生类中实现具体初始化
        }

        // 添加Job到管理器
        public void AddJob(TJob job) => jobs.Add(job);

        // 批量执行所有的Job
        public void ExecuteBatchJobs()
        {
            if (jobs.Count == 0)
            {
                return; // 如果没有Job，直接返回
            }

            int totalDataCount = 0;
            foreach (var job in jobs)
            {
                totalDataCount += job.DataCount; // 累计所有Job的数据总量
            }

            // 初始化批处理数据数组
            batchDataArray = new NativeArray<TData>(totalDataCount, Allocator.TempJob);

            // 批量拷贝数据到 batchDataArray 中
            int currentIndex = 0;
            for (int i = 0; i < jobs.Count; i++)  // 使用 for 循环代替 foreach
            {
                var job = jobs[i];
                var jobSlice = GetJobDataSlice(job); // 获取 job 的数据切片
                JobBatchManagerUnsafe.BatchCopy(ref batchDataArray, currentIndex, jobSlice);
                currentIndex += job.DataCount;
            }

            // 批量处理
            var batchJob = new MyBatchJob
            {
                dataArray = batchDataArray
            };
            var handle = batchJob.Schedule(totalDataCount, 64); // 每64个元素为一个批次
            handle.Complete();

            // 将结果数据批量拷贝回各自的Job
            currentIndex = 0;
            for (int i = 0; i < jobs.Count; i++)  // 使用 for 循环代替 foreach
            {
                var job = jobs[i];
                int jobDataCount = job.DataCount;
                var slice = new NativeSlice<TData>(batchDataArray, currentIndex, jobDataCount);
                SetJobResultBatch(ref job, slice);  // 使用 ref 传递 job

                currentIndex += jobDataCount;
            }

            batchDataArray.Dispose(); // 释放内存
        }

        // 清理所有已经执行的Jobs
        public void ClearJobs() => jobs.Clear();

        // 获取Job的数据切片（需要在派生类中实现）
        protected abstract NativeSlice<TData> GetJobDataSlice(TJob job);

        // 批量设置Job的结果（派生类实现该方法以处理批量结果）
        protected abstract void SetJobResultBatch(ref TJob job, NativeSlice<TData> resultSlice);

        // 内部定义的批处理Job
        [BurstCompile]
        private struct MyBatchJob : IJobParallelFor
        {
            public NativeArray<TData> dataArray; // 批处理数据

            public void Execute(int index)
            {
                // 假设我们进行一个简单的数学运算
                var data = dataArray[index];
                // 执行具体的计算逻辑
                // 此处省略，派生类需要实现具体逻辑
                dataArray[index] = data;
            }
        }
    }
}

