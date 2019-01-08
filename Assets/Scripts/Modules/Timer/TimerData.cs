using System;
namespace FoxGame.Timer {
	internal class TimerData : AbsTimerData {
		private Action ActionHandler_;
		public override Delegate ActionHandler {
			get {
				return this.ActionHandler_;
			}
			set {
				this.ActionHandler_ = (value as Action);
			}
		}
		public override void DoAction() {
			this.ActionHandler_();
		}
	}

	internal class TimerData<T> : AbsTimerData {
		private Action<T> ActionHandler_;
		private T Arg1_;
		public override Delegate ActionHandler {
			get {
				return this.ActionHandler_;
			}
			set {
				this.ActionHandler_ = (value as Action<T>);
			}
		}
		public T Arg1 {
			get {
				return this.Arg1_;
			}
			set {
				this.Arg1_ = value;
			}
		}
		public override void DoAction() {
			this.ActionHandler_(this.Arg1_);
		}
	}

	internal class TimerData<T, U> : AbsTimerData {
		private Action<T, U> ActionHandler_;
		private T Arg1_;
		private U Arg2_;
		public override Delegate ActionHandler {
			get {
				return this.ActionHandler_;
			}
			set {
				this.ActionHandler_ = (value as Action<T, U>);
			}
		}
		public T Arg1 {
			get {
				return this.Arg1_;
			}
			set {
				this.Arg1_ = value;
			}
		}
		public U Arg2 {
			get {
				return this.Arg2_;
			}
			set {
				this.Arg2_ = value;
			}
		}
		public override void DoAction() {
			this.ActionHandler_(this.Arg1_, this.Arg2_);
		}
	}

	internal class TimerData<T, U, V> : AbsTimerData {
		private Action<T, U, V> ActionHandler_;
		private T Arg1_;
		private U Arg2_;
		private V Arg3_;
		public override Delegate ActionHandler {
			get {
				return this.ActionHandler_;
			}
			set {
				this.ActionHandler_ = (value as Action<T, U, V>);
			}
		}
		public T Arg1 {
			get {
				return this.Arg1_;
			}
			set {
				this.Arg1_ = value;
			}
		}
		public U Arg2 {
			get {
				return this.Arg2_;
			}
			set {
				this.Arg2_ = value;
			}
		}
		public V Arg3 {
			get {
				return this.Arg3_;
			}
			set {
				this.Arg3_ = value;
			}
		}
		public override void DoAction() {
			this.ActionHandler_(this.Arg1_, this.Arg2_, this.Arg3_);
		}
	}
}
