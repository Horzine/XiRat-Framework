using System;
using System.Collections;
using System.Collections.Generic;

namespace Xi.Extend.Collection
{
    internal static class CollectionHelpers
    {
        public static IReadOnlyCollection<T> ReifyCollection<T>(IEnumerable<T> source)
        {
            return source == null
                ? throw new ArgumentNullException(nameof(source))
                : source is IReadOnlyCollection<T> result
                ? result
                : source is ICollection<T> collection
                ? new CollectionWrapper<T>(collection)
                : (global::System.Collections.Generic.IReadOnlyCollection<T>)(source is ICollection nongenericCollection ? new NongenericCollectionWrapper<T>(nongenericCollection) : new List<T>(source));
        }

        private sealed class NongenericCollectionWrapper<T> : IReadOnlyCollection<T>
        {
            private readonly ICollection _collection;

            public NongenericCollectionWrapper(ICollection collection) => _collection = collection ?? throw new ArgumentNullException(nameof(collection));

            public int Count => _collection.Count;

            public IEnumerator<T> GetEnumerator()
            {
                foreach (T item in _collection)
                {
                    yield return item;
                }
            }

            IEnumerator IEnumerable.GetEnumerator() => _collection.GetEnumerator();
        }

        private sealed class CollectionWrapper<T> : IReadOnlyCollection<T>
        {
            private readonly ICollection<T> _collection;

            public CollectionWrapper(ICollection<T> collection) => _collection = collection ?? throw new ArgumentNullException(nameof(collection));

            public int Count => _collection.Count;

            public IEnumerator<T> GetEnumerator() => _collection.GetEnumerator();

            IEnumerator IEnumerable.GetEnumerator() => _collection.GetEnumerator();
        }
    }
}