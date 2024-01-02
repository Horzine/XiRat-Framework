using System;
using System.Collections;
using System.Collections.Generic;

namespace Xi.Extend.Collection
{
    public class IndexedSet<T> : IList<T>
    {
        private readonly List<T> _elements = new();
        private readonly Dictionary<T, int> _hashedElementIdx = new();

        public void Add(T item)
        {
            if (!_hashedElementIdx.ContainsKey(item))
            {
                int index = _elements.Count;
                _hashedElementIdx.Add(item, index);
                _elements.Add(item);
            }
        }

        public bool Remove(T item)
        {
            if (_hashedElementIdx.TryGetValue(item, out int index))
            {
                RemoveAt(index);
                return true;
            }

            return false;
        }

        public void Remove(Predicate<T> match)
        {
            _elements.RemoveAll(match);
            RefreshIndices();
        }

        public void Clear()
        {
            _elements.Clear();
            _hashedElementIdx.Clear();
        }

        public bool Contains(T item) => _hashedElementIdx.ContainsKey(item);

        public int Count => _elements.Count;

        public bool IsReadOnly => false;

        public int IndexOf(T item) => _hashedElementIdx.TryGetValue(item, out int index) ? index : -1;

        public void Insert(int index, T item) => throw new NotSupportedException("Random Insertion is semantically invalid, since this structure does not guarantee ordering.");

        public void RemoveAt(int index)
        {
            var removedItem = _elements[index];
            _elements.RemoveAt(index);
            _hashedElementIdx.Remove(removedItem);

            if (index != _elements.Count)
            {
                var lastItem = _elements[index];
                _hashedElementIdx[lastItem] = index;
            }
        }

        public T this[int index]
        {
            get => _elements[index];
            set
            {
                var oldItem = _elements[index];
                _hashedElementIdx.Remove(oldItem);

                _elements[index] = value;
                _hashedElementIdx.Add(value, index);
            }
        }

        public void Sort(Comparison<T> sortLayoutFunction)
        {
            _elements.Sort(sortLayoutFunction);
            RefreshIndices();
        }

        public IEnumerator<T> GetEnumerator() => _elements.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        void ICollection<T>.CopyTo(T[] array, int arrayIndex) => _elements.CopyTo(array, arrayIndex);

        private void RefreshIndices()
        {
            _hashedElementIdx.Clear();
            for (int i = 0; i < _elements.Count; ++i)
            {
                var item = _elements[i];
                _hashedElementIdx.Add(item, i);
            }
        }
    }
}
