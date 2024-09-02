using UnityEngine;
using UnityEngine.Assertions;
using Xi.Extension.Collection;

namespace Xi.TestCase
{
    public class Test_IndexedSet : MonoBehaviour
    {
        private void Start()
        {
            print("Test Start");

            AddElement_AddsToSet();

            RemoveElement_RemovesFromSet();

            RemoveNonExistentElement_ReturnsFalse();

            RemoveByPredicate_RemovesMatchingElements();

            Clear_EmptySet();

            IndexOf_ReturnsCorrectIndex();

            Sort_SortsElements();

            GetEnumerator_ReturnsEnumerator();

            print("Test End");

            var set = new IndexedSet<int>
            {
                0,1,2,3,4,5,6,
            };

            set.RemoveAt(0);

        }

        public void AddElement_AddsToSet()
        {
            var set = new IndexedSet<int>
            {
                1
            };
            Assert.IsTrue(set.Contains(1));
            Assert.AreEqual(1, set.Count);
        }

        public void RemoveElement_RemovesFromSet()
        {
            var set = new IndexedSet<int>
            {
                1
            };
            Assert.IsTrue(set.Remove(1));
            Assert.IsFalse(set.Contains(1));
            Assert.AreEqual(0, set.Count);
        }

        public void RemoveNonExistentElement_ReturnsFalse()
        {
            var set = new IndexedSet<int>();
            Assert.IsFalse(set.Remove(1));
        }

        public void RemoveByPredicate_RemovesMatchingElements()
        {
            var set = new IndexedSet<int>
            {
                1,
                2,
                3
            };

            set.Remove(x => x % 2 == 0);

            Assert.IsFalse(set.Contains(2));
            Assert.IsTrue(set.Contains(1));
            Assert.IsTrue(set.Contains(3));
            Assert.AreEqual(2, set.Count);
        }

        public void Clear_EmptySet()
        {
            var set = new IndexedSet<int>
            {
                1,
                2
            };

            set.Clear();

            Assert.AreEqual(0, set.Count);
            Assert.IsFalse(set.Contains(1));
            Assert.IsFalse(set.Contains(2));
        }

        public void IndexOf_ReturnsCorrectIndex()
        {
            var set = new IndexedSet<int>
            {
                1,
                2,
                3
            };

            Assert.AreEqual(0, set.IndexOf(1));
            Assert.AreEqual(1, set.IndexOf(2));
            Assert.AreEqual(2, set.IndexOf(3));
            Assert.AreEqual(-1, set.IndexOf(4));
        }

        public void Sort_SortsElements()
        {
            var set = new IndexedSet<int>
            {
                3,
                1,
                2
            };

            set.Sort((a, b) => a.CompareTo(b));

            Assert.AreEqual(1, set[0]);
            Assert.AreEqual(2, set[1]);
            Assert.AreEqual(3, set[2]);
        }

        public void GetEnumerator_ReturnsEnumerator()
        {
            var set = new IndexedSet<int>
            {
                1,
                2,
                3
            };

            var enumerator = set.GetEnumerator();

            Assert.IsTrue(enumerator.MoveNext());
            Assert.AreEqual(1, enumerator.Current);

            enumerator.MoveNext();
            Assert.AreEqual(2, enumerator.Current);

            enumerator.MoveNext();
            Assert.AreEqual(3, enumerator.Current);

            Assert.IsFalse(enumerator.MoveNext());
        }
    }
}