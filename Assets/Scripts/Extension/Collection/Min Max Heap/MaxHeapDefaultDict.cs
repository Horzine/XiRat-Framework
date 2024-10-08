﻿using System.Collections;
using System.Collections.Generic;

namespace Xi.Extension.Collection
{
    // A wrapper of MaxHeap<TKey, TValue, TDictionary>.
    // Uses Dictionary<TKey, int> as TDictionary.
    //
    public class MaxHeap<TKey, TValue> : IReadOnlyDictionary<TKey, TValue>
    {
        private readonly MaxHeap<TKey, TValue, Dictionary<TKey, int>> heap;

        public MaxHeap(IEnumerable<KeyValuePair<TKey, TValue>> items,
            IComparer<TValue> comparer) => heap = new MaxHeap<TKey, TValue, Dictionary<TKey, int>>(items, comparer);

        public MaxHeap(IEnumerable<KeyValuePair<TKey, TValue>> items)
            : this(items, Comparer<TValue>.Default)
        { }

        public MaxHeap(IComparer<TValue> comparer)
            : this(new KeyValuePair<TKey, TValue>[0], comparer)
        { }

        public MaxHeap() : this(Comparer<TValue>.Default) { }

        public int Count => heap.Count;

        public KeyValuePair<TKey, TValue> Max => heap.Max;

        public IEnumerable<TKey> Keys => heap.Keys;

        public IEnumerable<TValue> Values => heap.Values;

        /// <summary>
        /// Gets the value correspoinding to the given key.
        /// </summary>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="KeyNotFoundException"></exception>
        public TValue this[TKey key] => heap[key];

        /// <summary>
        /// Extract the largest element.
        /// </summary>
        /// <exception cref="InvalidOperationException"></exception>
        public KeyValuePair<TKey, TValue> ExtractMax() => heap.ExtractMax();

        /// <summary>
        /// Insert the key and value.
        /// </summary>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentException"></exception>
        public void Add(TKey key, TValue val) => heap.Add(key, val);

        /// <summary>
        /// Modify the value corresponding to the given key.
        /// </summary>
        /// <exception cref="ArgumentNullException">Key is null.</exception>
        /// <exception cref="KeyNotFoundException"></exception>
        public void ChangeValue(TKey key, TValue newValue) => heap.ChangeValue(key, newValue);

        /// <summary>
        /// Returns whether the key exists.
        /// </summary>
        /// <exception cref="ArgumentNullException"></exception>
        public bool ContainsKey(TKey key) => heap.ContainsKey(key);

        public bool TryGetValue(TKey key, out TValue value) => heap.TryGetValue(key, out value);

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator() => heap.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
