using System;
using System.Collections.Generic;

namespace AgnosticSearch {
    public class PriorityList<V, T> where V : IComparable {
        private List<PrioritisedItem> _items { get; set; } = new List<PrioritisedItem>();

        public T this[int index] {
            get {
                if (_items == null) return default(T);
                return _items[index].Value;
            }
        }

        public int Count { get => _items.Count; }

        public int Insert(V priority, T value) {
            if (_items == null) _items = new List<PrioritisedItem>();
            if (_items.Count == 0) {
                _items.Add(new PrioritisedItem(priority, value));
                return 0;
            }
            return Insert(0, _items.Count - 1, new PrioritisedItem(priority, value));
        }

        public bool RemoveAt(int index) {
            if (_items == null) return false;
            if (_items.Count > index) {
                _items.RemoveAt(index);
                return true;
            }
            return false;
        }

        public V Next {
            get {
                return _items[_items.Count - 1].Priority;
            }
        }

        private int Insert(int indexA, int indexB, PrioritisedItem item) {
            int index = indexA + (indexB - indexA) / 2;
            // Last index? Compare & insert
            if (indexA == indexB) {
                if (item.Priority.CompareTo(_items[index].Priority) < 0) {
                    index++;
                }
                _items.Insert(index, item);
                return index;
            }
            // Not last index? Partition & iterate
            if (item.Priority.CompareTo(_items[index].Priority) > 0) {
                if (index == 0) {
                    _items.Insert(index, item);
                    return index;
                }
                indexB = index;
            } else {
                if (index == _items.Count - 1) {
                    _items.Insert(++index, item);
                    return index;
                }
                indexA = index + 1;
            }
            return Insert(indexA, indexB, item);
        }

        private struct PrioritisedItem {
            public V Priority { get; private set; }
            public T Value { get; private set; }
            public PrioritisedItem(V priority, T value) {
                Priority = priority;
                Value = value;
            }
        }
    }
}