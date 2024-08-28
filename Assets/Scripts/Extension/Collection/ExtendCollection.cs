using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Xi.Extension.Collection
{
    public static partial class ExtendCollection
    {
        public struct Pair<K, V>
        {
            public Pair(K k, V v)
            {
                _k = k;
                _v = v;
            }
            public K _k;
            public V _v;
        }
        public static void Foreach(int max, Action<int> fun)
        {
            for (int index = 0; index < max; index++)
            {
                fun(index);
            }
        }
        public static void Foreach(int outerMax, int innerMax, Action<int, int> fun)
        {
            for (int x = 0; x < outerMax; x++)
            {
                for (int y = 0; y < innerMax; y++)
                {
                    fun(x, y);
                }
            }
        }
        public static T Find<T>(this IEnumerable<T> enu, Func<T, bool> match) => enu.FirstOrDefault(match);
        public static T Find<T>(this T[] array, Predicate<T> match) => Array.Find(array, match);
        public static bool Exist<T>(this IEnumerable<T> enu, Func<T, bool> match) => enu.Any(match);
        public static bool Exist<T>(this T[] array, Predicate<T> match) => Array.Exists(array, match);
        public static int Count<T>(this T[] array, Predicate<T> match) => Array.FindAll(array, match).Length;
        public static void Foreach<T>(this T[] array, Action<T> action) => Array.ForEach(array, action);
        public static void Foreach<K, V>(this Dictionary<K, V> dict, Action<KeyValuePair<K, V>> fun)
        {
            foreach (var it in dict)
            {
                fun(it);
            }
        }
        public static void ForeachKey<K, V>(this Dictionary<K, V> dict, Action<K> fun)
        {
            foreach (var key in dict.Keys)
            {
                fun(key);
            }
        }
        public static void ForeachValue<K, V>(this Dictionary<K, V> dict, Action<V> fun)
        {
            foreach (var value in dict.Values)
            {
                fun(value);
            }
        }
    }

    public class ForeachMutableList<T> : ICollection<T>, IEnumerable<T>, IEnumerable
    {
        private readonly List<T> _added = new();
        private readonly List<T> _current = new();
        private readonly List<T> _removed = new();
        private bool _isSynced = true;

        public int Count
        {
            get
            {
                Sync();
                return _current.Count;
            }
        }

        public bool IsReadOnly => false;

        public void Add(T item)
        {
            if (_removed.Contains(item))
            {
                _removed.Remove(item);
            }

            _added.Add(item);
            _isSynced = false;
        }

        public void Clear()
        {
            _added.Clear();
            _removed.Clear();
            _current.Clear();
            _isSynced = true;
        }

        public bool Contains(T item)
        {
            Sync();
            return !_removed.Contains(item) && (_current.Contains(item) || _added.Contains(item));
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            Sync();
            _current.CopyTo(array, arrayIndex);
        }

        public IEnumerator<T> GetEnumerator()
        {
            Sync();
            return _current.GetEnumerator();
        }

        public bool Remove(T item)
        {
            bool removed = false;
            if (_added.Contains(item))
            {
                _added.Remove(item);
                removed = true;
            }

            if (_current.Contains(item))
            {
                removed = true;
                _removed.Add(item);
            }

            _isSynced = false;
            return removed;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            Sync();
            return _current.GetEnumerator();
        }

        public T this[int index]
        {
            get
            {
                Sync();
                return _current[index];
            }
        }

        private void Sync()
        {
            if (_isSynced)
            {
                return;
            }

            _current.RemoveAll(_removed.Contains);
            _removed.Clear();

            foreach (var item in _added)
            {
                if (!_current.Contains(item))
                {
                    _current.Add(item);
                }
            }

            _added.Clear();
            _isSynced = true;
        }

        int ICollection<T>.Count => Count;
        bool ICollection<T>.IsReadOnly => IsReadOnly;
        void ICollection<T>.Add(T item) => Add(item);
        void ICollection<T>.Clear() => Clear();
        bool ICollection<T>.Contains(T item) => Contains(item);
        void ICollection<T>.CopyTo(T[] array, int arrayIndex) => CopyTo(array, arrayIndex);
        bool ICollection<T>.Remove(T item) => Remove(item);
        IEnumerator<T> IEnumerable<T>.GetEnumerator() => GetEnumerator();
    }
}