using System.Collections.Generic;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;
using Xi.Tools;

namespace Xi.Extension.Collection
{
    public interface IQuadtreeObject<T> where T : IQuadtreeObject<T>
    {
        internal Rect GetBounds(); // 返回对象的边界
        internal Quadtree<T> CurrentQuadtree { get; set; } // 记录当前所在的四叉树
        internal Rect LastInsertCachedBound { get; set; }
    }

    public class Quadtree<T> where T : IQuadtreeObject<T>
    {
        private const int kDefaultMaxObjectsPerNode = 10;
        private const int kDefaultMaxLevels = 5;
        private readonly int _maxObjectsPerNode;
        private readonly int _maxLevels;
        private readonly int _level;
        private IndexedSet<T> _objectSet = new();

        public Rect Bounds { get; private set; }
        private readonly Quadtree<T> _parentNode;
        public Quadtree<T>[] Nodes { get; private set; } = new Quadtree<T>[4];

        public bool HasSubNode => Nodes[0] != null;
        private readonly Stack<Quadtree<T>> _stackForRetrieve = new();
        private readonly List<T> _listForRetrieve = new();

        public static Quadtree<T> Create(Rect bounds,
            Quadtree<T> _parentNode = null,
            int maxObjectsPerNode = kDefaultMaxObjectsPerNode,
            int maxLevels = kDefaultMaxLevels)
            => new(0, bounds, _parentNode, maxObjectsPerNode, maxLevels);

        private Quadtree(int level, Rect bounds, Quadtree<T> parentNodeNode, int maxObjectsPerNode = kDefaultMaxObjectsPerNode, int maxLevels = kDefaultMaxLevels)
        {
            _level = level;
            Bounds = bounds;
            _maxObjectsPerNode = maxObjectsPerNode;
            _maxLevels = maxLevels;
            _parentNode = parentNodeNode;
        }

        private void Split()
        {
            float subWidth = Bounds.width / 2f;
            float subHeight = Bounds.height / 2f;
            float x = Bounds.x;
            float y = Bounds.y;

            Nodes[0] = new Quadtree<T>(_level + 1,
                new Rect(x + subWidth, y, subWidth, subHeight),
                this,
                maxObjectsPerNode: _maxObjectsPerNode,
                maxLevels: _maxLevels);
            Nodes[1] = new Quadtree<T>(_level + 1,
                new Rect(x, y, subWidth, subHeight),
                this,
                maxObjectsPerNode: _maxObjectsPerNode,
                maxLevels: _maxLevels);
            Nodes[2] = new Quadtree<T>(_level + 1,
                new Rect(x, y + subHeight, subWidth, subHeight),
                this,
                maxObjectsPerNode: _maxObjectsPerNode,
                maxLevels: _maxLevels);
            Nodes[3] = new Quadtree<T>(_level + 1,
                new Rect(x + subWidth, y + subHeight, subWidth, subHeight),
                this,
                maxObjectsPerNode: _maxObjectsPerNode,
                maxLevels: _maxLevels);
        }

        private int GetIndex(Rect bounds)
        {
            int index = -1;
            float verticalMidpoint = Bounds.x + (Bounds.width / 2f);
            float horizontalMidpoint = Bounds.y + (Bounds.height / 2f);

            bool topQuadrant = bounds.y < horizontalMidpoint && bounds.y + bounds.height < horizontalMidpoint;
            bool bottomQuadrant = bounds.y > horizontalMidpoint;

            if (bounds.x < verticalMidpoint && bounds.x + bounds.width < verticalMidpoint)
            {
                index = topQuadrant ? 1 : (bottomQuadrant ? 2 : -1);
            }
            else if (bounds.x > verticalMidpoint)
            {
                index = topQuadrant ? 0 : (bottomQuadrant ? 3 : -1);
            }

            return index;
        }

        public void Insert(T obj, Rect? currentBounds = null)
        {
            var bounds = currentBounds ?? obj.GetBounds();

            // 处理边界扩展
            if (!Bounds.Overlaps(bounds))
            {
                Expand(bounds);
                XiLogger.LogWarning($"Quadtree is Expand and Rebuild a New Quadtree, newRect is {Bounds}");
            }

            if (HasSubNode)
            {
                int index = GetIndex(bounds);

                if (index != -1)
                {
                    Nodes[index].Insert(obj, bounds);
                    return;
                }
            }

            // 检查是否已经存在于当前四叉树
            if (!_objectSet.Contains(obj))
            {
                _objectSet.Add(obj);
                obj.CurrentQuadtree = this; // 更新当前四叉树引用
                obj.LastInsertCachedBound = bounds;// 缓存当前的 bound

                // 如果达到最大对象数并且没有达到最大层数，进行分割
                if (_objectSet.Count > _maxObjectsPerNode && _level < _maxLevels)
                {
                    if (Nodes[0] == null)
                    {
                        Split();
                    }

                    // 重新插入当前对象
                    for (int i = _objectSet.Count - 1; i >= 0; i--)
                    {
                        // 这里36和47在同一帧移动了， 目前更新的是36号，但是这里原本的47号位置也改变了所以取到了新的位置
                        // 47号新的位置不在原本的 Rect 框内，于是触发了下面 insert 的扩容
                        // 为了解决这个问题：修改为构建某个格子的时候， 需要缓存当前的 bound 信息，这里重现插入的时候不要用新的而是用缓存的值
                        var objectBounds = _objectSet[i].LastInsertCachedBound;
                        int index = GetIndex(objectBounds);
                        if (index != -1)
                        {
                            Nodes[index].Insert(_objectSet[i], objectBounds);
                            _objectSet.RemoveAt(i);
                        }
                    }
                }
            }
        }

        private void Expand(Rect newBounds)
        {
            // 限制扩展比例，避免无限增大
            float expansionFactor = 2f;
            float newWidth = Mathf.Max(Bounds.width, newBounds.width) * expansionFactor;
            float newHeight = Mathf.Max(Bounds.height, newBounds.height) * expansionFactor;

            var newRect = new Rect
            (
                Bounds.x - ((newWidth - Bounds.width) / 2f),
                Bounds.y - ((newHeight - Bounds.height) / 2f),
                newWidth,
                newHeight
            );

            var newRoot = new Quadtree<T>(_level, newRect, null, maxObjectsPerNode: _maxObjectsPerNode, maxLevels: _maxLevels);
            // 将现有对象重新插入新节点
            InsertAllObjectsIntoNewNode(this, newRoot);

            _objectSet = newRoot._objectSet;
            Nodes = newRoot.Nodes;
            Bounds = newRoot.Bounds;
        }

        private void InsertAllObjectsIntoNewNode(Quadtree<T> node, Quadtree<T> newRoot)
        {
            foreach (var obj in node._objectSet)
            {
                newRoot.Insert(obj, obj.LastInsertCachedBound);
            }

            if (node.HasSubNode)
            {
                for (int i = 0; i < node.Nodes.Length; i++)
                {
                    if (node.Nodes[i] != null)
                    {
                        InsertAllObjectsIntoNewNode(node.Nodes[i], newRoot);
                    }
                }
            }
        }

        public bool Remove(T obj)
        {
            if (_objectSet.Contains(obj))
            {
                return _objectSet.Remove(obj);
            }

            if (HasSubNode)
            {
                var bounds = obj.GetBounds();
                int index = GetIndex(bounds);
                if (index != -1)
                {
                    return Nodes[index].Remove(obj);
                }
            }

            return false;
        }

        public void UpdateObject(T obj)
        {
            if (obj.CurrentQuadtree == null)
            {
                XiLogger.LogError($"obj not in Quadtree");
                return;
            }

            var curQuadTree = obj.CurrentQuadtree;

            bool removeSuccess = curQuadTree.Remove(obj);
            Insert(obj);

            if (removeSuccess)
            {
                curQuadTree.CheckForShrinkage();
            }
        }

        // 检查是否需要缩容
        private void CheckForShrinkage()
        {
            if (_parentNode != null)
            {
                int totalObjectsInParentNode = _parentNode.GetTotalObjectCount();

                // 检查父节点的总对象数是否<=阈值
                if (totalObjectsInParentNode <= _maxObjectsPerNode)
                {
                    _parentNode.Shrink();
                }
            }
        }

        private void Shrink()
        {
            for (int i = 0; i < Nodes.Length; i++)
            {
                var item = Nodes[i];
                if (item == null)
                {
                    continue;
                }

                var nodeObjectSet = item._objectSet;
                foreach (var obj in nodeObjectSet)
                {
                    _objectSet.Add(obj);
                    obj.CurrentQuadtree = this;
                }

                nodeObjectSet.Clear();
                Nodes[i] = null;
            }
        }

        private int GetTotalObjectCount()
        {
            int count = _objectSet.Count;

            // 递归计算所有子节点的对象总数
            foreach (var node in Nodes)
            {
                if (node != null)
                {
                    count += node.GetTotalObjectCount();
                }
            }

            return count;
        }

        public void ClearObjects()
        {
            foreach (var obj in _objectSet)
            {
                obj.CurrentQuadtree = null;
            }

            _objectSet.Clear();
            for (int i = 0; i < Nodes.Length; i++)
            {
                Nodes[i]?.ClearObjects();
            }
        }

        public void ClearQuadtree()
        {
            foreach (var obj in _objectSet)
            {
                obj.CurrentQuadtree = null;
            }

            _objectSet.Clear();
            for (int i = 0; i < Nodes.Length; i++)
            {
                if (Nodes[i] != null)
                {
                    Nodes[i].ClearQuadtree();
                    Nodes[i] = null;
                }
            }
        }

        public List<T> Retrieve(Rect bounds)
        {
            var returnObjects = new List<T>();

            if (!Bounds.Overlaps(bounds))
            {
                return returnObjects; // 当前节点的边界和目标边界不相交
            }

            _stackForRetrieve.Clear();
            _stackForRetrieve.Push(this);

            while (_stackForRetrieve.Count > 0)
            {
                var node = _stackForRetrieve.Pop();

                // 检查当前节点是否与目标边界相交
                if (node.Bounds.Overlaps(bounds))
                {
                    foreach (var item in node._objectSet)
                    {
                        if (bounds.Overlaps(item.GetBounds()))
                        {
                            returnObjects.Add(item);
                        }
                    }

                    if (node.HasSubNode)
                    {
                        foreach (var child in node.Nodes)
                        {
                            _stackForRetrieve.Push(child);
                        }
                    }
                }
            }

            return returnObjects;
        }

        public void RetrieveNonAlloc(List<T> result, Rect bounds)
        {
            if (result == null)
            {
                XiLogger.LogError("result is null");
                return;
            }

            result.Clear();

            if (!Bounds.Overlaps(bounds))
            {
                return; // 当前节点的边界和目标边界不相交
            }

            _stackForRetrieve.Clear();
            _stackForRetrieve.Push(this);

            _listForRetrieve.Clear();

            while (_stackForRetrieve.Count > 0)
            {
                var node = _stackForRetrieve.Pop();

                // 检查当前节点是否与目标边界相交
                if (node.Bounds.Overlaps(bounds))
                {
                    _listForRetrieve.AddRange(node._objectSet);

                    if (node.HasSubNode)
                    {
                        foreach (var child in node.Nodes)
                        {
                            _stackForRetrieve.Push(child);
                        }
                    }
                }
            }

            var nodeBoundsArray = new NativeArray<Rect>(_listForRetrieve.Count, Allocator.TempJob);
            var resultArray = new NativeArray<bool>(_listForRetrieve.Count, Allocator.TempJob);

            for (int i = 0; i < _listForRetrieve.Count; i++)
            {
                nodeBoundsArray[i] = _listForRetrieve[i].GetBounds();
            }

            var targetBounds = new float4(bounds.xMin, bounds.yMin, bounds.xMax, bounds.yMax);

            var overlapJob = new OverlapJob
            {
                NodeBoundsArray = nodeBoundsArray,
                TargetBounds = targetBounds,
                ResultArray = resultArray
            };

            var handle = overlapJob.Schedule(_listForRetrieve.Count, 64);
            handle.Complete();

            for (int i = 0; i < resultArray.Length; i++)
            {
                if (resultArray[i])
                {
                    result.Add(_listForRetrieve[i]);
                }
            }

            nodeBoundsArray.Dispose();
            resultArray.Dispose();
        }

        [BurstCompile]
        private struct OverlapJob : IJobParallelFor
        {
            [ReadOnly] public NativeArray<Rect> NodeBoundsArray;
            [ReadOnly] public float4 TargetBounds; // 使用 float4 进行计算（xMin, yMin, xMax, yMax）
            public NativeArray<bool> ResultArray;

            public void Execute(int index)
            {
                var bounds = NodeBoundsArray[index];
                var nodeBounds = new float4(bounds.xMin, bounds.yMin, bounds.xMax, bounds.yMax);
                ResultArray[index] = CheckOverlap(nodeBounds, TargetBounds);
            }

            private static bool CheckOverlap(float4 a, float4 b) => a.z > b.x && a.x < b.z && a.w > b.y && a.y < b.w;
        }
    }
}
