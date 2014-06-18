using System;
using System.Collections.Generic;
using System.Linq;

namespace Ark.Collections {
    /// <summary>
    /// A priority queue of item lists.
    /// You enqueue elements that you want to process in future.
    /// When you enqueue elements, you specify a delay (the priority tier) of the item which defines how many times do you need to switch to the next priority tier before you see the item again.
    /// After processing the current list of items, switch to the next one.
    /// </summary>
    /// <typeparam name="T">Specifies the type of elements in the queue.</typeparam>
    public class DelayQueue<T> {
        List<T>[] _queue;
        int _current;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Ark.Collections.DelayQueue`1{T}"/> class that is empty and has the default initial number of priority tiers.
        /// </summary>
        public DelayQueue()
            : this(4) {
        }
        
        /// <summary>
        /// Initializes a new instance of the <see cref="T:Ark.Collections.DelayQueue`1{T}"/> class that is empty and has the specified initial number of priority tiers.
        /// </summary>
        /// <param name="capacity">Initial number of priority tiers.</param>
        public DelayQueue(int capacity = 4) {
            if (capacity < 0) {
                throw new ArgumentOutOfRangeException("capacity", "The number must be greater than or equal to zero.");
            }
            _queue = new List<T>[capacity];
        }

        /// <summary>
        /// The list of items belonging to the current priority tier.
        /// </summary>
        public IEnumerable<T> CurrentList {
            get {
                return _queue[_current] ?? Enumerable.Empty<T>();
            }
        }

        /// <summary>
        /// Adds an object to the queue.
        /// </summary>
        /// <param name="item">The object to add to the queue.</param>
        /// <param name="delay">The relative priority.</param>
        public void Enqueue(T item, int delay) {
            if (delay < 0) {
                throw new ArgumentOutOfRangeException("delay", "The number must be greater than or equal to zero.");
            }
            EnsureCapacity(delay + 1);
            var idx = (_current + delay) % _queue.Length;
            var list = _queue[idx];
            if (list == null) {
                _queue[idx] = list = new List<T>();
            }
            list.Add(item);
        }

        void EnsureCapacity(int capacity) {
            if (capacity < _queue.Length) {
                return;
            }
            int newCapacity = _queue.Length;
            while (newCapacity < capacity) {
                newCapacity *= 2;
            }
            List<T>[] newQueue = new List<T>[newCapacity];
            Array.Copy(_queue, _current, newQueue, 0, _queue.Length - _current);
            Array.Copy(_queue, 0, newQueue, _queue.Length - _current, _current);
            _queue = newQueue;
            _current = 0;
        }

        /// <summary>
        /// Switch to the next priority tier.
        /// </summary>
        public void SwitchToNextList() {
            if (_queue[_current] != null) {
                _queue[_current].Clear();
            }
            _current = (_current + 1) % _queue.Length;
        }
    }
}
