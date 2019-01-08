using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
namespace MS {
	[Serializable]
	public class KeyedPriorityQueue<K, V, P> where V : class {
		[Serializable]
		private struct HeapNode<KK, VV, PP> {
			public KK Key;
			public VV Value;
			public PP Priority;
			public HeapNode(KK key, VV value, PP priority) {
				this.Key = key;
				this.Value = value;
				this.Priority = priority;
			}
		}
		private List<KeyedPriorityQueue<K, V, P>.HeapNode<K, V, P>> heap;
		private KeyedPriorityQueue<K, V, P>.HeapNode<K, V, P> placeHolder;
		private Comparer<P> priorityComparer;
		private int size;
		public event EventHandler<KeyedPriorityQueueHeadChangedEventArgs<V>> FirstElementChanged;
		public int Count {
			get {
				return this.size;
			}
		}
		public ReadOnlyCollection<K> Keys {
			get {
				List<K> list = new List<K>();
				for (int i = 1; i <= this.size; i++) {
					list.Add(this.heap[i].Key);
				}
				return new ReadOnlyCollection<K>(list);
			}
		}
		public ReadOnlyCollection<V> Values {
			get {
				List<V> list = new List<V>();
				for (int i = 1; i <= this.size; i++) {
					list.Add(this.heap[i].Value);
				}
				return new ReadOnlyCollection<V>(list);
			}
		}
		public KeyedPriorityQueue() {
			this.heap = new List<KeyedPriorityQueue<K, V, P>.HeapNode<K, V, P>>();
			this.priorityComparer = Comparer<P>.Default;
			this.placeHolder = default(KeyedPriorityQueue<K, V, P>.HeapNode<K, V, P>);
			this.heap.Add(default(KeyedPriorityQueue<K, V, P>.HeapNode<K, V, P>));
		}
		public void Clear() {
			this.heap.Clear();
			this.size = 0;
		}
		public V Dequeue() {
			V result = (this.size < 1) ? default(V) : this.DequeueImpl();
			V newHead = (this.size < 1) ? default(V) : this.heap[1].Value;
			this.RaiseHeadChangedEvent(default(V), newHead);
			return result;
		}
		private V DequeueImpl() {
			V value = this.heap[1].Value;
			this.heap[1] = this.heap[this.size];
			this.heap[this.size--] = this.placeHolder;
			this.Heapify(1);
			return value;
		}
		public void Enqueue(K key, V value, P priority) {
			V v = (this.size > 0) ? this.heap[1].Value : default(V);
			int num = ++this.size;
			int num2 = num / 2;
			if (num == this.heap.Count) {
				this.heap.Add(this.placeHolder);
			}
			while (num > 1 && this.IsHigher(priority, this.heap[num2].Priority)) {
				this.heap[num] = this.heap[num2];
				num = num2;
				num2 = num / 2;
			}
			this.heap[num] = new KeyedPriorityQueue<K, V, P>.HeapNode<K, V, P>(key, value, priority);
			V value2 = this.heap[1].Value;
			if (!value2.Equals(v)) {
				this.RaiseHeadChangedEvent(v, value2);
			}
		}
		public V FindByPriority(P priority, Predicate<V> match) {
			V result;
			if (this.size >= 1) {
				result = this.Search(priority, 1, match);
			} else {
				result = default(V);
			}
			return result;
		}
		private void Heapify(int i) {
			int num = 2 * i;
			int num2 = num + 1;
			int num3 = i;
			if (num <= this.size && this.IsHigher(this.heap[num].Priority, this.heap[i].Priority)) {
				num3 = num;
			}
			if (num2 <= this.size && this.IsHigher(this.heap[num2].Priority, this.heap[num3].Priority)) {
				num3 = num2;
			}
			if (num3 != i) {
				this.Swap(i, num3);
				this.Heapify(num3);
			}
		}
		protected virtual bool IsHigher(P p1, P p2) {
			return this.priorityComparer.Compare(p1, p2) < 1;
		}
		public V Peek() {
			V result;
			if (this.size >= 1) {
				result = this.heap[1].Value;
			} else {
				result = default(V);
			}
			return result;
		}
		private void RaiseHeadChangedEvent(V oldHead, V newHead) {
			if (oldHead != newHead) {
				EventHandler<KeyedPriorityQueueHeadChangedEventArgs<V>> firstElementChanged = this.FirstElementChanged;
				if (firstElementChanged != null) {
					firstElementChanged(this, new KeyedPriorityQueueHeadChangedEventArgs<V>(oldHead, newHead));
				}
			}
		}
		public V Remove(K key) {
			V result;
			if (this.size >= 1) {
				V value = this.heap[1].Value;
				for (int i = 1; i <= this.size; i++) {
					K key2 = this.heap[i].Key;
					if (key2.Equals(key)) {
						V value2 = this.heap[i].Value;
						this.Swap(i, this.size);
						this.heap[this.size--] = this.placeHolder;
						this.Heapify(i);
						V value3 = this.heap[1].Value;
						if (!value.Equals(value3)) {
							this.RaiseHeadChangedEvent(value, value3);
						}
						result = value2;
						return result;
					}
				}
			}
			result = default(V);
			return result;
		}
		private V Search(P priority, int i, Predicate<V> match) {
			V v = default(V);
			if (this.IsHigher(this.heap[i].Priority, priority)) {
				if (match(this.heap[i].Value)) {
					v = this.heap[i].Value;
				}
				int num = 2 * i;
				int num2 = num + 1;
				if (v == null && num <= this.size) {
					v = this.Search(priority, num, match);
				}
				if (v == null && num2 <= this.size) {
					v = this.Search(priority, num2, match);
				}
			}
			return v;
		}
		private void Swap(int i, int j) {
			KeyedPriorityQueue<K, V, P>.HeapNode<K, V, P> value = this.heap[i];
			this.heap[i] = this.heap[j];
			this.heap[j] = value;
		}
	}
}
