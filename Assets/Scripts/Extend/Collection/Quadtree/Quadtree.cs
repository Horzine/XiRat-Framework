using System.Collections.Generic;
using UnityEngine;
using Xi.Tools;

namespace Xi.Extend.Collection
{
    public interface IQuadtreeObject
    {
        internal Rect GetBounds(); // 返回对象的边界
    }

    public class Quadtree<T> where T : IQuadtreeObject
    {
        private const int kDefaultMaxObjectsPerNode = 10;
        private const int kDefaultMaxLevels = 5;
        private readonly int _maxObjectsPerNode;
        private readonly int _maxLevels;
        private readonly int _level;
        private List<T> _objects = new();
        private Rect _bounds;
        public Rect Bounds => _bounds;
        public Quadtree<T>[] Nodes { get; private set; } = new Quadtree<T>[4];

        private readonly Stack<Quadtree<T>> _stack = new(); // 栈用于检索迭代

        public static Quadtree<T> Create(Rect bounds, int maxObjectsPerNode = kDefaultMaxObjectsPerNode, int maxLevels = kDefaultMaxLevels)
            => new(0, bounds, maxObjectsPerNode, maxLevels);

        private Quadtree(int level, Rect bounds, int maxObjectsPerNode = kDefaultMaxObjectsPerNode, int maxLevels = kDefaultMaxLevels)
        {
            _level = level;
            _bounds = bounds;
            _maxObjectsPerNode = maxObjectsPerNode;
            _maxLevels = maxLevels;
        }

        public void Clear()
        {
            _objects.Clear();
            for (int i = 0; i < Nodes.Length; i++)
            {
                if (Nodes[i] != null)
                {
                    Nodes[i].Clear();
                    Nodes[i] = null;
                }
            }
        }

        private void Split()
        {
            float subWidth = _bounds.width / 2f;
            float subHeight = _bounds.height / 2f;
            float x = _bounds.x;
            float y = _bounds.y;

            Nodes[0] = new Quadtree<T>(_level + 1, new Rect(x + subWidth, y, subWidth, subHeight), _maxObjectsPerNode, _maxLevels);
            Nodes[1] = new Quadtree<T>(_level + 1, new Rect(x, y, subWidth, subHeight), _maxObjectsPerNode, _maxLevels);
            Nodes[2] = new Quadtree<T>(_level + 1, new Rect(x, y + subHeight, subWidth, subHeight), _maxObjectsPerNode, _maxLevels);
            Nodes[3] = new Quadtree<T>(_level + 1, new Rect(x + subWidth, y + subHeight, subWidth, subHeight), _maxObjectsPerNode, _maxLevels);
        }

        private int GetIndex(Rect bounds)
        {
            int index = -1;
            float verticalMidpoint = _bounds.x + (_bounds.width / 2f);
            float horizontalMidpoint = _bounds.y + (_bounds.height / 2f);

            bool topQuadrant = bounds.y < horizontalMidpoint && bounds.y + bounds.height < horizontalMidpoint;
            bool bottomQuadrant = bounds.y > horizontalMidpoint;

            if (bounds.x < verticalMidpoint && bounds.x + bounds.width < verticalMidpoint)
            {
                if (topQuadrant)
                {
                    index = 1;
                }
                else if (bottomQuadrant)
                {
                    index = 2;
                }
            }
            else if (bounds.x > verticalMidpoint)
            {
                if (topQuadrant)
                {
                    index = 0;
                }
                else if (bottomQuadrant)
                {
                    index = 3;
                }
            }

            return index;
        }

        public void Insert(T obj)
        {
            var bounds = obj.GetBounds();

            // 处理边界扩展
            if (!_bounds.Overlaps(bounds))
            {
                Expand(bounds);
            }

            if (Nodes[0] != null)
            {
                int index = GetIndex(bounds);

                if (index != -1)
                {
                    Nodes[index].Insert(obj);
                    return;
                }
            }

            _objects.Add(obj);

            // 如果达到最大对象数并且没有达到最大层数，进行分割
            if (_objects.Count > _maxObjectsPerNode && _level < _maxLevels)
            {
                if (Nodes[0] == null)
                {
                    Split();
                }

                for (int i = 0; i < _objects.Count; i++)
                {
                    var objectBounds = _objects[i].GetBounds();
                    int index = GetIndex(objectBounds);
                    if (index != -1)
                    {
                        Nodes[index].Insert(_objects[i]);
                        _objects.RemoveAt(i);
                        i--; // 确保索引正确
                    }
                }
            }
        }

        private void Expand(Rect newBounds)
        {
            // 限制扩展比例，避免无限增大
            float expansionFactor = 2f;
            float newWidth = Mathf.Max(_bounds.width, newBounds.width) * expansionFactor;
            float newHeight = Mathf.Max(_bounds.height, newBounds.height) * expansionFactor;

            var newRect = new Rect
            (
                _bounds.x - ((newWidth - _bounds.width) / 2f),
                _bounds.y - ((newHeight - _bounds.height) / 2f),
                newWidth,
                newHeight
            );

            var newRoot = new Quadtree<T>(_level, newRect, _maxObjectsPerNode, _maxLevels);
            foreach (var obj in _objects)
            {
                newRoot.Insert(obj); // 将现有对象重新插入新节点
            }

            _objects = newRoot._objects;
            Nodes = newRoot.Nodes;
            _bounds = newRoot._bounds;
        }

        public void Remove(T obj)
        {
            var bounds = obj.GetBounds();
            if (Nodes[0] != null)
            {
                int index = GetIndex(bounds);
                if (index != -1)
                {
                    Nodes[index].Remove(obj);
                    return;
                }
            }

            _objects.Remove(obj);
        }

        public List<T> Retrieve(Rect bounds)
        {
            var returnObjects = new List<T>();

            if (!_bounds.Overlaps(bounds))
            {
                return returnObjects;
            }

            _stack.Clear();
            _stack.Push(this);

            while (_stack.Count > 0)
            {
                var node = _stack.Pop();

                returnObjects.AddRange(node._objects.FindAll(obj => bounds.Overlaps(obj.GetBounds())));

                if (node.Nodes[0] != null)
                {
                    foreach (var child in node.Nodes)
                    {
                        _stack.Push(child);
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

            if (!_bounds.Overlaps(bounds))
            {
                return;
            }

            _stack.Clear();
            _stack.Push(this);

            while (_stack.Count > 0)
            {
                var node = _stack.Pop();

                result.AddRange(node._objects);

                if (node.Nodes[0] != null)
                {
                    foreach (var child in node.Nodes)
                    {
                        _stack.Push(child);
                    }
                }
            }

            result.RemoveAll(obj => !bounds.Overlaps(obj.GetBounds()));
        }
    }
}
