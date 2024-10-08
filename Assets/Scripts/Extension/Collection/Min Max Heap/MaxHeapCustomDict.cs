﻿using System.Collections;
using System.Collections.Generic;

namespace Xi.Extension.Collection
{
    /// <typeparam name="TDictionary">
    /// Maps a key to the index of the corresponding KeyValuePair 
    /// in the list.</typeparam>
    public class MaxHeap<TKey, TValue, TDictionary> : IReadOnlyDictionary<TKey, TValue>
        where TDictionary : IDictionary<TKey, int>, new()
    {
        private readonly MinHeap<TKey, TValue, TDictionary> minHeap;

        public MaxHeap(IEnumerable<KeyValuePair<TKey, TValue>> items, IComparer<TValue> comparer)
        {
            var negatedComparer = Comparer<TValue>.Create(
                (x, y) => comparer.Compare(y, x));

            minHeap = new MinHeap<TKey, TValue, TDictionary>(items, negatedComparer);
        }

        public MaxHeap(IEnumerable<KeyValuePair<TKey, TValue>> items)
            : this(items, Comparer<TValue>.Default)
        { }

        public MaxHeap(IComparer<TValue> comparer)
            : this(new KeyValuePair<TKey, TValue>[0], comparer)
        { }

        public MaxHeap() : this(Comparer<TValue>.Default) { }

        public int Count => minHeap.Count;

        public KeyValuePair<TKey, TValue> Max => minHeap.Min;

        public IEnumerable<TKey> Keys => minHeap.Keys;

        public IEnumerable<TValue> Values => minHeap.Values;

        /// <summary>
        /// Gets the value correspoinding to the given key.
        /// </summary>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="KeyNotFoundException"></exception>
        public TValue this[TKey key] => minHeap[key];

        /// <summary>
        /// Extract the largest element.
        /// </summary>
        /// <exception cref="InvalidOperationException"></exception>
        public KeyValuePair<TKey, TValue> ExtractMax() => minHeap.ExtractMin();

        /// <summary>
        /// Insert the key and value.
        /// </summary>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentException"></exception>
        public void Add(TKey key, TValue val) => minHeap.Add(key, val);

        /// <summary>
        /// Modify the value corresponding to the given key.
        /// </summary>
        /// <exception cref="ArgumentNullException">Key is null.</exception>
        /// <exception cref="KeyNotFoundException"></exception>
        public void ChangeValue(TKey key, TValue newValue) => minHeap.ChangeValue(key, newValue);

        /// <summary>
        /// Returns whether the key exists.
        /// </summary>
        /// <exception cref="ArgumentNullException"></exception>
        public bool ContainsKey(TKey key) => minHeap.ContainsKey(key);

        public bool TryGetValue(TKey key, out TValue value) => minHeap.TryGetValue(key, out value);

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator() => minHeap.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    }
}
