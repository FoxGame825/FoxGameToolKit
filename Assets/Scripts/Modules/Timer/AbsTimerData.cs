using System;
namespace FoxGame.Timer {
	internal abstract class AbsTimerData {
		private uint TimerID_;
		private int ActionInterval_;
		private ulong UnNextTimeMS_; // 下一次执行action的毫秒 tick
		public uint TimerID {
			get {
				return this.TimerID_;
			}
			set {
				this.TimerID_ = value;
			}
		}
		public int ActionInterval {
			get {
				return this.ActionInterval_;
			}
			set {
				this.ActionInterval_ = value;
			}
		}
		public ulong UnNextTimeMS {
			get {
				return this.UnNextTimeMS_;
			}
			set {
				this.UnNextTimeMS_ = value;
			}
		}
		public abstract Delegate ActionHandler {
			get;
			set;
		}
		public abstract void DoAction();
	}
}
