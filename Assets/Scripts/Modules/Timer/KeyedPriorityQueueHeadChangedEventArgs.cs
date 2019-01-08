using System;
namespace MS {
	public sealed class KeyedPriorityQueueHeadChangedEventArgs<T> : EventArgs where T : class {
		private T newFirstElement;
		private T oldFirstElement;
		public T NewFirstElement {
			get {
				return this.newFirstElement;
			}
		}
		public T OldFirstElement {
			get {
				return this.oldFirstElement;
			}
		}
		public KeyedPriorityQueueHeadChangedEventArgs(T oldFirstElement, T newFirstElement) {
			this.oldFirstElement = oldFirstElement;
			this.newFirstElement = newFirstElement;
		}
	}
}
