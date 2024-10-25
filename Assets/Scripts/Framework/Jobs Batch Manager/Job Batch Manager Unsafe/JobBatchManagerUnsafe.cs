using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;

namespace Xi.Framework
{
    public static class JobBatchManagerUnsafe
    {
        public static void Copy<TData>(int i, ref NativeArray<TData> batchDataArray, ref TData tempData) where TData : unmanaged
        {
            unsafe
            {
                void* basePtr = batchDataArray.GetUnsafePtr();
                void* targetPtr = (byte*)basePtr + (i * UnsafeUtility.SizeOf<TData>());
                UnsafeUtility.MemCpy(targetPtr, UnsafeUtility.AddressOf(ref tempData), UnsafeUtility.SizeOf<TData>());
            }
        }

        public static unsafe void BatchCopy<TData>(ref NativeArray<TData> batchDataArray, int startIndex, NativeSlice<TData> sourceSlice) where TData : unmanaged
        {
            // 获取 batchDataArray 和 sourceSlice 的底层指针
            void* basePtr = batchDataArray.GetUnsafePtr();
            void* targetPtr = (byte*)basePtr + (startIndex * UnsafeUtility.SizeOf<TData>());

            // 获取 sourceSlice 的底层指针
            void* sourcePtr = sourceSlice.GetUnsafePtr();

            // 计算需要拷贝的字节数
            int totalSize = sourceSlice.Length * UnsafeUtility.SizeOf<TData>();

            // 执行内存拷贝
            UnsafeUtility.MemCpy(targetPtr, sourcePtr, totalSize);
        }
    }
}
